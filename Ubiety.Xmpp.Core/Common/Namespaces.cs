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

namespace Ubiety.Xmpp.Core.Common
{
    /// <summary>
    ///     XML namespaces for tags
    /// </summary>
    public static class Namespaces
    {
#pragma warning disable S1075 // URIs should not be hardcoded
        /// <summary>
        ///     Stream namespace
        /// </summary>
        public const string Stream = "http://etherx.jabber.org/streams";
#pragma warning restore S1075 // URIs should not be hardcoded

        /// <summary>
        ///     Client namespace
        /// </summary>
        public const string Client = "jabber:client";

        /// <summary>
        ///     XMPP streams namespace
        /// </summary>
        public const string XmppStreams = "urn:ietf:params:xml:ns:xmpp-streams";

        /// <summary>
        ///     XMPP TLS namespace
        /// </summary>
        public const string Tls = "urn:ietf:params:xml:ns:xmpp-tls";
    }
}
