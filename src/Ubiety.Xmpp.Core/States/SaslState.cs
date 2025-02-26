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
    /// Represents the SASL (Simple Authentication and Security Layer) state in the XMPP workflow.
    /// Handles SASL authentication mechanisms and transitions the XMPP client to the appropriate state
    /// based on the outcome of the authentication process.
    /// </summary>
    public class SaslState : IState
    {
        /// <summary>
        /// Executes the SASL authentication process based on the provided XMPP client state and tag.
        /// </summary>
        /// <param name="xmpp">The XMPP client performing the authentication.</param>
        /// <param name="tag">An optional tag representing the current stage of the SASL authentication process.</param>
        public void Execute(XmppBase xmpp, Tag tag = null)
        {
            if (xmpp is XmppClient client)
            {
                switch (tag)
                {
                    case Success:
                        client.ClientSocket.SetReadClear();
                        client.Authenticated = true;
                        client.State = new ConnectedState();
                        client.State.Execute(client);
                        break;

                    case Failure:
                        client.State = new DisconnectState();
                        client.State.Execute(client);
                        break;

                    default:
                        client.ClientSocket.SetReadClear();
                        client.ClientSocket.Send(client.SaslProcessor.Step(tag));
                        break;
                }
            }
        }
    }
}
