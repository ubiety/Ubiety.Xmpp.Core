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
using Ubiety.Xmpp.Core.Tags.Sasl;

namespace Ubiety.Xmpp.Core.States
{
    /// <summary>
    ///     SASL XMPP state
    /// </summary>
    public class SaslState : IState
    {
        /// <summary>
        ///     Execute the the state
        /// </summary>
        /// <param name="xmpp">Xmpp client to use</param>
        /// <param name="tag">Tag from the server</param>
        public void Execute(XmppBase xmpp, Tag tag = null)
        {
            if (xmpp is XmppClient client)
            {
                var result = xmpp.SaslProcessor.Step(tag);
                switch (result)
                {
                    case Success s:
                        xmpp.ClientSocket.SetReadClear();
                        client.Authenticated = true;
                        client.State = new ConnectedState();
                        client.State.Execute(client);
                        break;
                    case Failure f:
                        client.State = new DisconnectState();
                        client.State.Execute(client);
                        break;
                    default:
                        xmpp.ClientSocket.SetReadClear();
                        client.ClientSocket.Send(result);
                        break;
                }
            }
        }
    }
}
