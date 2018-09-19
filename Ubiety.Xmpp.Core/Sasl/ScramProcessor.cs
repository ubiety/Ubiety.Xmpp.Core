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
using System.Globalization;
using System.Linq;
using System.Security.Authentication.ExtendedProtection;
using System.Security.Cryptography;
using System.Text;
using StringPrep;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Infrastructure.Extensions;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Sasl.Scram;
using Ubiety.Xmpp.Core.Stringprep;
using Ubiety.Xmpp.Core.Tags;
using Ubiety.Xmpp.Core.Tags.Sasl;

namespace Ubiety.Xmpp.Core.Sasl
{
    /// <summary>
    ///     SCRAM-SHA-1 SASL Processor
    /// </summary>
    public class ScramProcessor : SaslProcessor
    {
        private readonly bool _channelBinding;
        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly IPreparationProcess _saslprep = SaslprepProfile.Create();
        private ClientFinalMessage _clientFinalMessage;
        private ClientFirstMessage _clientFirstMessage;
        private ILog _logger;
        private ServerMessage _serverMessage;
        private byte[] _serverSignature;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScramProcessor" /> class
        /// </summary>
        /// <param name="channelBinding">Do we want to use channel binding</param>
        public ScramProcessor(bool channelBinding)
        {
            _channelBinding = channelBinding;
        }

        /// <summary>
        ///     Initializes the SASL processor
        /// </summary>
        /// <param name="id"><see cref="Jid" /> of the user for the session</param>
        /// <param name="password">Password of the user</param>
        /// <returns>Next tag to send to the server</returns>
        public override Tag Initialize(Jid id, string password)
        {
            base.Initialize(id, password);

            _logger = Log.Get<ScramProcessor>();
            _logger.Log(LogLevel.Debug, "Initializing SCRAM SASL processor");

            var nonce = NextInt64().ToString(CultureInfo.InvariantCulture);

            _clientFirstMessage = new ClientFirstMessage(Id.User, nonce, _channelBinding);

            var auth = Client.Registry.GetTag<Auth>(Auth.XmlName);
            auth.MechanismType = _channelBinding ? MechanismTypes.ScramPlus : MechanismTypes.Scram;
            auth.Bytes = _encoding.GetBytes(_clientFirstMessage.FirstAuthMessage);

            return auth;
        }

        /// <summary>
        ///     Process the next SASL step
        /// </summary>
        /// <param name="tag">Tag received from the server</param>
        /// <returns>Next tag to send to the server</returns>
        public override Tag Step(Tag tag)
        {
            switch (tag)
            {
                case Challenge c:
                    _logger.Log(LogLevel.Debug, "Received challenge");
                    return ProcessChallenge(c);
                case Response s:
                    var response = _encoding.GetString(s.Bytes);
                    var signature = Convert.FromBase64String(response.Substring(2));
                    return _encoding.GetString(signature) == _encoding.GetString(_serverSignature) ? s : null;
                case Failure f:
                    return f;
                default:
                    return default(Tag);
            }
        }

        private Tag ProcessChallenge(Challenge tag)
        {
            var response = _encoding.GetString(tag.Bytes);

            _serverMessage = ServerMessage.ParseResponse(response);

            if (_channelBinding)
            {
                var binding = Client.ClientSocket.TransportContext.GetChannelBinding(ChannelBindingKind.Unique);
                _logger.Log(LogLevel.Debug, $"Channel data: {binding}");
                _clientFinalMessage = new ClientFinalMessage(_clientFirstMessage, _serverMessage, binding);
                _logger.Log(LogLevel.Debug, $"Channel header: {_clientFinalMessage.Channel}");
            }
            else
            {
                _clientFinalMessage = new ClientFinalMessage(_clientFirstMessage, _serverMessage);
            }

            CalculateProofs();

            _logger.Log(LogLevel.Debug, $"Client final after proof: {_clientFinalMessage.Message}");

            var message = Client.Registry.GetTag<Response>(Response.XmlName);
            message.Bytes = _encoding.GetBytes(_clientFinalMessage.Message);

            return message;
        }

        private void CalculateProofs()
        {
            var hmac = new HMACSHA1();
            SHA1 hash = new SHA1CryptoServiceProvider();

            var saltedPassword = Hi();

            hmac.Key = saltedPassword;
            var clientKey = hmac.ComputeHash(_encoding.GetBytes("Client Key"));
            var serverKey = hmac.ComputeHash(_encoding.GetBytes("Server Key"));
            var storedKey = hash.ComputeHash(clientKey);

            var authMessage =
                $"{_clientFirstMessage.BareMessage},{ServerMessage.FirstMessage},{_clientFinalMessage.MessageWithoutProof}";
            _logger.Log(LogLevel.Debug, $"Auth message: {authMessage}");
            var auth = _encoding.GetBytes(authMessage);

            hmac.Key = storedKey;
            var signature = hmac.ComputeHash(auth);

            hmac.Key = serverKey;
            _serverSignature = hmac.ComputeHash(auth);

            var proof = clientKey.ExclusiveOr(signature);

            _clientFinalMessage.SetProof(proof);
        }

        private byte[] Hi()
        {
            var password = _encoding.GetBytes(_saslprep.Run(Password));
            var completeSalt = _serverMessage.Salt.Value.Concat(BitConverter.GetBytes(1)).ToArray();

            var hmac = new HMACSHA1(password);
            var iteration = hmac.ComputeHash(completeSalt);
            var result = iteration;

            for (var i = 0; i < _serverMessage.Iterations.Value; ++i)
            {
                iteration = hmac.ComputeHash(iteration);
                result = result.ExclusiveOr(iteration);
            }

            return result;
        }
    }
}