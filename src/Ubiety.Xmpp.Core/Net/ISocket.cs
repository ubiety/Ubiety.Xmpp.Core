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
using Ubiety.Xmpp.Core.Common;

namespace Ubiety.Xmpp.Core.Net
{
    /// <summary>
    /// Represents a socket interface used for communication with an XMPP server.
    /// </summary>
    public interface ISocket
    {
        /// <summary>
        /// Raised when data is received on the socket connection.
        /// </summary>
        event EventHandler<DataEventArgs> Data;

        /// <summary>
        /// Event triggered when a connection is established or changes state in the socket interface.
        /// </summary>
        event EventHandler Connection;

        /// <summary>
        /// Gets a value indicating whether the socket connection to the XMPP server is currently active.
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// Connects to the server using the specified JID.
        /// </summary>
        /// <param name="jid">The Jabber Identifier (JID) of the user.</param>
        void Connect(Jid jid);

        /// <summary>
        /// Disconnects from the server and cleans up related resources.
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Sends a message to the connected XMPP server.
        /// </summary>
        /// <param name="message">The message to send to the server.</param>
        void Send(string message);
    }
}
