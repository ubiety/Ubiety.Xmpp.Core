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
    ///     State to disconnect from the server
    /// </summary>
    /// <inheritdoc />
    public class DisconnectState : IState
    {
        private static readonly ILog Logger;

        static DisconnectState()
        {
            Logger = Log.Get<DisconnectState>();
        }

        /// <inheritdoc />
        public void Execute(XmppBase xmpp, Tag tag = null)
        {
            Logger.Log(LogLevel.Debug, "Disconnecting from the server");
            xmpp.ClientSocket.Disconnect();
            xmpp.State = new DisconnectedState();
        }
    }
}
