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

using Ubiety.Scram.Core;
using Ubiety.Scram.Core.Messages;
using Ubiety.Stringprep.Core;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Infrastructure.Attributes;
using Ubiety.Xmpp.Core.Infrastructure.Exceptions;
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
    [Sasl("SCRAM-SHA1", typeof(ScramProcessor), 30, false, MechanismTypes.Scram1)]
    [Sasl("SCRAM-SHA1-PLUS", typeof(ScramProcessor), 35, true, MechanismTypes.Scram1Plus)]
    [Sasl("SCRAM-SHA256", typeof(ScramProcessor), 40, false, MechanismTypes.Scram256)]
    [Sasl("SCRAM-SHA256-PLUS", typeof(ScramProcessor), 45, true, MechanismTypes.Scram256Plus)]
    [Sasl("SCRAM-SHA512", typeof(ScramProcessor), 50, false, MechanismTypes.Scram512)]
    [Sasl("SCRAM-SHA512-PLUS", typeof(ScramProcessor), 55, true, MechanismTypes.Scram512Plus)]
    public class ScramProcessor : SaslProcessor
    {
        private static readonly ILog Logger = Log.Get<ScramProcessor>();
        private readonly IPreparationProcess _saslprep = SaslprepProfile.Create();
        private ClientFinalMessage _clientFinalMessage;
        private ClientFirstMessage _clientFirstMessage;
        private ServerFirstMessage _serverFirstMessage;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScramProcessor" /> class.
        /// </summary>
        public ScramProcessor()
        {
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

            _clientFirstMessage = new ClientFirstMessage(_saslprep.Run(Id.User), nonce, ChannelBinding ? ChannelBindingStatus.Required : ChannelBindingStatus.NotSupported);
            Logger.Log(LogLevel.Debug, _clientFirstMessage.Message);

            var auth = Client.TagRegistry.GetTag<Auth>(Auth.XmlName);
            auth.MechanismType = MechanismType;
            auth.Bytes = _clientFirstMessage;

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
                    ServerFinalMessage serverFinalMessage = s.Bytes;
                    return serverFinalMessage.ServerSignature == _clientFinalMessage.ServerSignature ? s : null;

                case Failure f:
                    return f;

                default:
                    return null;
            }
        }

        private Response ProcessChallenge(Challenge tag)
        {
            _serverFirstMessage = tag.Bytes;

            var hash = MechanismType switch
            {
                MechanismTypes.Scram1 => Hash.Sha1(),
                MechanismTypes.Scram1Plus => Hash.Sha1(),
                MechanismTypes.Scram256 => Hash.Sha256(),
                MechanismTypes.Scram256Plus => Hash.Sha256(),
                MechanismTypes.Scram512 => Hash.Sha512(),
                MechanismTypes.Scram512Plus => Hash.Sha512(),
                _ => throw new InvalidTypeException()
            };

            _clientFinalMessage = new ClientFinalMessage(_clientFirstMessage, _serverFirstMessage, Password, hash);

            var message = Client.TagRegistry.GetTag<Response>(Response.XmlName);
            message.Bytes = _clientFinalMessage;

            return message;
        }
    }
}
