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
    ///     Defines a socket interface
    /// </summary>
    public interface ISocket
    {
        /// <summary>
        ///     Gets a value indicating whether the socket is connected
        /// </summary>
        bool Connected { get; }

        /// <summary>
        ///     Raised when data is received from the server
        /// </summary>
        event EventHandler<DataEventArgs> Data;

        /// <summary>
        ///     Raised when the socket is connected to the server
        /// </summary>
        event EventHandler Connection;

        /// <summary>
        ///     Connect to an XMPP server
        /// </summary>
        /// <param name="jid"><see cref="Jid"/> of the user</param>
        void Connect(Jid jid);

        /// <summary>
        ///     Disconnects from the server
        /// </summary>
        void Disconnect();

        /// <summary>
        ///     Send a message to the server
        /// </summary>
        /// <param name="message">Message to send</param>
        void Send(string message);
    }
}