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

using System.Linq;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Sasl.Scram.Parts;

namespace Ubiety.Xmpp.Core.Sasl.Scram
{
    /// <summary>
    ///     SCRAM SASL server messages
    /// </summary>
    public class ServerMessage
    {
        private readonly ILog _logger = Log.Get<ServerMessage>();

        private ServerMessage(IterationPart iterations, NoncePart nonce, SaltPart salt)
        {
            _logger.Log(LogLevel.Debug, $"Server first message: {FirstMessage}");

            Iterations = iterations;
            Nonce = nonce;
            Salt = salt;
        }

        /// <summary>
        ///     Gets the servers first message
        /// </summary>
        public static string FirstMessage { get; private set; }

        /// <summary>
        ///     Gets the iterations
        /// </summary>
        internal IterationPart Iterations { get; }

        /// <summary>
        ///     Gets the nonce
        /// </summary>
        internal NoncePart Nonce { get; }

        /// <summary>
        ///     Gets the salt
        /// </summary>
        internal SaltPart Salt { get; }

        /// <summary>
        ///     Parse the server response
        /// </summary>
        /// <param name="response">Response from the server</param>
        /// <returns>Server message</returns>
        public static ServerMessage ParseResponse(string response)
        {
            FirstMessage = response;
            var parts = ScramPart.ParseAll(response.Split(','));

            var iterations = parts.OfType<IterationPart>().ToList();
            var nonces = parts.OfType<NoncePart>().ToList();
            var salts = parts.OfType<SaltPart>().ToList();

            return new ServerMessage(iterations.First(), nonces.First(), salts.First());
        }
    }
}