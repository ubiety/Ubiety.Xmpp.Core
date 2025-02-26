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

using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.States;

namespace Ubiety.Xmpp.Core
{
    /// <summary>
    /// Represents the client interface for establishing and managing XMPP connections.
    /// </summary>
    public interface IClient
    {
        /// <summary>
        /// Gets or sets the JID (Jabber ID) associated with the client.
        /// </summary>
        Jid Id { get; set; }

        /// <summary>
        /// Gets or sets the password used to authenticate the client with the XMPP server.
        /// </summary>
        string Password { get; set; }

        /// <summary>
        /// Gets or sets the port number used for the XMPP connection.
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// Gets a value indicating whether SSL is used for the XMPP connection.
        /// </summary>
        bool UseSsl { get; }

        /// <summary>
        /// Gets a value indicating whether IPv6 should be used for network communication.
        /// </summary>
        bool UseIPv6 { get; }

        /// <summary>
        /// Gets a value indicating whether the client is authenticated with the XMPP server.
        /// </summary>
        bool Authenticated { get; }

        /// <summary>
        /// Gets the current state of the XMPP client within the workflow execution.
        /// </summary>
        IState State { get; }

        /// <summary>
        /// Establishes a connection to an XMPP server using the specified Jabber Identifier (JID) and password.
        /// </summary>
        /// <param name="jid">The <see cref="Jid"/> to use for connecting, which identifies the user and server.</param>
        /// <param name="password">The password associated with the JID to authenticate the connection.</param>
        void Connect(Jid jid, string password);
    }
}
