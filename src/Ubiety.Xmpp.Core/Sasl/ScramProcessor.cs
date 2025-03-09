// Copyright 2018 Dieter Lunn
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System;
using System.Collections.Generic;
using System.Text;
using Ubiety.Scram.Core;
using Ubiety.Scram.Core.Messages;
using Ubiety.Stringprep.Core;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Stringprep;
using Ubiety.Xmpp.Core.Tags;
using Ubiety.Xmpp.Core.Tags.Sasl;

namespace Ubiety.Xmpp.Core.Sasl
{
    /// <summary>
    /// Represents a SCRAM SASL (Simple Authentication and Security Layer) processor.
    /// </summary>
    /// <remarks>
    /// SCRAM (Salted Challenge Response Authentication Mechanism) is an authentication protocol
    /// used to securely authenticate a client to a server using a challenge-response mechanism.
    /// This processor handles the initialization and step processing for the SCRAM mechanism.
    /// </remarks>
    public class ScramProcessor : SaslProcessor
    {
        private static readonly ILog Logger = Log.Get<ScramProcessor>();
        private readonly bool _channelBinding;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly IPreparationProcess _saslprep = SaslprepProfile.Create();
        private ClientFinalMessage _clientFinalMessage;
        private ClientFirstMessage _clientFirstMessage;
        private ServerFirstMessage _serverFirstMessage;
        private string _serverResponse;
        private List<byte> _serverSignature;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScramProcessor" /> class.
        /// </summary>
        /// <param name="channelBinding">Do we want to use channel binding?.</param>
        public ScramProcessor(bool channelBinding)
        {
            _channelBinding = channelBinding;
        }

        /// <summary>
        /// Initializes the SCRAM SASL processor with the specified user information.
        /// </summary>
        /// <param name="id">The <see cref="Jid" /> representing the user's identifier for the session.</param>
        /// <param name="password">The password of the user.</param>
        /// <returns>A <see cref="Tag" /> containing the next message to send to the server during SASL authentication.</returns>
        public override Tag Initialize(Jid id, string password)
        {
            base.Initialize(id, password);

            Logger.Log(LogLevel.Debug, "Initializing SCRAM SASL processor");

            var nonce = CreateNonce();

            _clientFirstMessage = new ClientFirstMessage(_saslprep.Run(Id.User), nonce);
            Logger.Log(LogLevel.Debug, _clientFirstMessage.Message);

            var auth = Client.Registry.GetTag<Auth>(Auth.XmlName);
            auth.MechanismType = _channelBinding ? MechanismTypes.ScramPlus : MechanismTypes.Scram;
            auth.Bytes = _encoding.GetBytes(_clientFirstMessage.Message);

            return auth;
        }

        /// <summary>
        /// Processes the given server tag and returns the appropriate next tag to send.
        /// </summary>
        /// <param name="tag">The tag received from the server to be processed.</param>
        /// <returns>The next tag to send to the server based on the received tag.</returns>
        public override Tag Step(Tag tag)
        {
            switch (tag)
            {
                case Challenge c:
                    Logger.Log(LogLevel.Debug, "Received challenge");
                    return ProcessChallenge(c);

                case Response s:
                    Logger.Log(LogLevel.Debug, "Received response");
                    var response = _encoding.GetString(s.Bytes);
                    var signature = Convert.FromBase64String(response[2..]);
                    return _encoding.GetString(signature) == _encoding.GetString(_serverSignature.ToArray()) ? s : null;

                case Failure f:
                    return f;

                default:
                    return null;
            }
        }

        private Response ProcessChallenge(Challenge tag)
        {
            _serverResponse = _encoding.GetString(tag.Bytes);

            _serverFirstMessage = ServerFirstMessage.Parse(_serverResponse);

            _clientFinalMessage = new ClientFinalMessage(_clientFirstMessage, _serverFirstMessage);

            CalculateProofs();

            var message = Client.Registry.GetTag<Response>(Response.XmlName);
            message.Bytes = _encoding.GetBytes(_clientFinalMessage.Message);

            return message;
        }

        private void CalculateProofs()
        {
            var hash = Hash.Sha1();

            var password = _saslprep.Run(Password);

            var saltedPassword = hash.ComputeHash(
                _encoding.GetBytes(password),
                _serverFirstMessage.Salt?.Value ?? throw new InvalidOperationException(),
                _serverFirstMessage.Iterations?.Value ?? throw new InvalidOperationException());

            var clientKey = hash.ComputeHash(_encoding.GetBytes("Client Key"), saltedPassword);
            var serverKey = hash.ComputeHash(_encoding.GetBytes("Server Key"), saltedPassword);
            var storedKey = hash.ComputeHash(clientKey);

            var authMessage =
                $"{_clientFirstMessage.BareMessage},{_serverResponse},{_clientFinalMessage.MessageWithoutProof}";
            var auth = _encoding.GetBytes(authMessage);

            var signature = hash.ComputeHash(auth, storedKey);
            _serverSignature = new List<byte>(hash.ComputeHash(auth, serverKey));

            var proof = clientKey.ExclusiveOr(signature);

            _clientFinalMessage.SetProof(proof);
        }
    }
}
