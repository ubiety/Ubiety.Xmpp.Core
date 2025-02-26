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
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core.States
{
    /// <summary>
    /// Represents the connecting state of an XMPP client within the connection state machine.
    /// </summary>
    /// <remarks>
    /// The <c>ConnectingState</c> is responsible for handling the action of establishing a connection
    /// to the server during the XMPP communication lifecycle.
    /// </remarks>
    public class ConnectingState : IState
    {
        private static readonly ILog Logger = Log.Get<ConnectingState>();

        /// <summary>
        /// Executes the connecting state logic for the XMPP client, initializing the connection to the server.
        /// </summary>
        /// <param name="xmpp">The XMPP base instance to execute the logic on.</param>
        /// <param name="tag">An optional tag that may contain additional information for execution.</param>
        public void Execute(XmppBase xmpp, Tag tag = null)
        {
            Logger.Log(LogLevel.Debug, "Executing ConnectingState");
            if (xmpp is XmppClient client)
            {
                Logger.Log(LogLevel.Debug, "Connecting to server");
                client.ClientSocket.Connect(client.Id);
                client.ClientSocket.SetReadClear();
            }
        }
    }
}
