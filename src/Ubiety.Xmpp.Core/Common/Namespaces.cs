// Copyright 2018, 2019 Dieter Lunn
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
    /// Represents a collection of namespace constants used in the XMPP (Extensible Messaging and Presence Protocol) communication.
    /// </summary>
    public static class Namespaces
    {
#pragma warning disable S1075 // URIs should not be hardcoded

        /// <summary>
        /// Represents the namespace URI for stream-level communication in the XMPP protocol.
        /// </summary>
        public const string Stream = "http://etherx.jabber.org/streams";

#pragma warning restore S1075 // URIs should not be hardcoded

        /// <summary>
        /// Represents the namespace URI for client-level communication in the XMPP protocol.
        /// </summary>
        public const string Client = "jabber:client";

        /// <summary>
        /// Represents the namespace URI specific to error conditions in the XMPP protocol.
        /// </summary>
        public const string XmppStreams = "urn:ietf:params:xml:ns:xmpp-streams";

        /// <summary>
        /// Represents the namespace URI for implementing TLS (Transport Layer Security) in the XMPP protocol.
        /// </summary>
        public const string Tls = "urn:ietf:params:xml:ns:xmpp-tls";

        /// <summary>
        /// Represents the namespace URI used for SASL (Simple Authentication and Security Layer) in the XMPP protocol.
        /// </summary>
        public const string Sasl = "urn:ietf:params:xml:ns:xmpp-sasl";

        /// <summary>
        /// Represents the namespace URI for resource binding in the XMPP protocol, as defined by RFC 6120.
        /// </summary>
        public const string Bind = "urn:ietf:params:xml:ns:xmpp-bind";
    }
}
