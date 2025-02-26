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
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core.States
{
    /// <summary>
    /// Represents a state where the Xmpp client is disconnected from the server.
    /// </summary>
    /// <remarks>
    /// This state indicates that no active connection to the server exists and no further actions
    /// are performed. It provides an implementation of the Execute method as required by the IState
    /// interface, which does not perform any operations in this state.
    /// </remarks>
    public class DisconnectedState : IState
    {
        /// <inheritdoc />
        public void Execute(XmppBase xmpp, Tag tag = null)
        {
            // Disconnected from a server - nothing to do
        }
    }
}
