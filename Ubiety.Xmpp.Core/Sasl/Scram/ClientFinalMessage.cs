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

using Ubiety.Xmpp.Core.Sasl.Scram.Parts;

namespace Ubiety.Xmpp.Core.Sasl.Scram
{
    /// <summary>
    ///     SCRAM client final message
    /// </summary>
    public class ClientFinalMessage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientFinalMessage" /> class
        /// </summary>
        /// <param name="firstMessage">Client first message</param>
        /// <param name="serverMessage">Server message</param>
        public ClientFinalMessage(ClientFirstMessage firstMessage, ServerMessage serverMessage)
        {
            Channel = new ChannelPart(firstMessage.Gs2Header);
            Nonce = new NoncePart(firstMessage.Nonce.Value, serverMessage.Nonce.Value);
        }

        /// <summary>
        ///     Gets the final message without the client proof
        /// </summary>
        public string MessageWithoutProof => $"{Channel},{Nonce}";

        /// <summary>
        ///     Gets the final message with client proof
        /// </summary>
        public string Message => $"{MessageWithoutProof},{ClientProof}";

        /// <summary>
        ///     Gets the nonce
        /// </summary>
        internal NoncePart Nonce { get; }

        /// <summary>
        ///     Gets the channel
        /// </summary>
        internal ChannelPart Channel { get; }

        /// <summary>
        ///     Gets the client proof
        /// </summary>
        internal ClientProofPart ClientProof { get; private set; }

        /// <summary>
        ///     Sets the client proof
        /// </summary>
        /// <param name="proof">Client proof</param>
        public void SetProof(byte[] proof)
        {
            ClientProof = new ClientProofPart(proof);
        }
    }
}