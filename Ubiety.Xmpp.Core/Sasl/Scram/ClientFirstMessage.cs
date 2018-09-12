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
    ///     SCRAM SASL client messages
    /// </summary>
    public class ClientFirstMessage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientFirstMessage"/> class
        /// </summary>
        /// <param name="username">Username of the user to authenticate</param>
        /// <param name="nonce">Nonce for the messages</param>
        /// <param name="channelBinding">Are we using channel binding</param>
        public ClientFirstMessage(string username, string nonce, bool channelBinding)
        {
            Username = new UsernamePart(username);
            Nonce = new NoncePart(nonce);
            ChannelBinding = channelBinding;
        }

        /// <summary>
        ///     Gets the GS2 header
        /// </summary>
        public string Gs2Header => ChannelBinding ? "p=tls-unique,," : "n,,";

        /// <summary>
        ///     Gets the client first message
        /// </summary>
        public string BareMessage => $"{Username},{Nonce}";

        /// <summary>
        ///     Gets the first client authentication message
        /// </summary>
        public string FirstAuthMessage => $"{Gs2Header},{BareMessage}";

        /// <summary>
        ///     Gets a value indicating whether we are using channel binding
        /// </summary>
        public bool ChannelBinding { get; }

        /// <summary>
        ///     Gets the username
        /// </summary>
        internal UsernamePart Username { get; }

        /// <summary>
        ///     Gets the nonce
        /// </summary>
        internal NoncePart Nonce { get; }
    }
}
