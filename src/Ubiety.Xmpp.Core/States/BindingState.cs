// Copyright 2019 Dieter Lunn
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
using System.Xml.Linq;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Tags;
using Ubiety.Xmpp.Core.Tags.Binding;
using Ubiety.Xmpp.Core.Tags.Client;

namespace Ubiety.Xmpp.Core.States
{
    /// <summary>
    /// Represents the state responsible for handling XMPP resource binding and transitioning during an XMPP session.
    /// </summary>
    public class BindingState : IState
    {
        /// <summary>
        /// Executes the resource binding operation within the XMPP session.
        /// </summary>
        /// <param name="xmpp">The XMPP base instance used for communication.</param>
        /// <param name="tag">The received tag to process, or null if initiating the bind operation.</param>
        /// <exception cref="InvalidOperationException">Thrown when an invalid IQ type is encountered.</exception>
        public void Execute(XmppBase xmpp, Tag tag = null)
        {
            var client = xmpp as XmppClient;

            if (tag is null)
            {
                var bind = xmpp.Registry.GetTag<Bind>(XName.Get("bind", Namespaces.Bind));
                var iq = xmpp.Registry.GetTag<Iq>(XName.Get("iq", Namespaces.Client));

                if (!string.IsNullOrEmpty(client?.Resource))
                {
                    var resource = xmpp.Registry.GetTag<Resource>(XName.Get("resource", Namespaces.Bind));
                    resource.Value = client.Resource;
                    bind.Add(resource);
                }

                iq.IqType = IqType.Set;
                iq.Add(bind);

                xmpp.ClientSocket.SetReadClear();
                xmpp.ClientSocket.Send(iq);
            }
            else
            {
                var iq = tag as Iq;

                switch (iq?.IqType)
                {
                    case IqType.Result:
                        if (client != null)
                        {
                            client.Id = iq.Bind.Jid.Id;
                            client.State = new ReadyState();
                            client.State.Execute(xmpp);
                        }

                        break;
                    case IqType.Error:
                        break;
                    default:
                        throw new InvalidOperationException("Invalid Iq type");
                }
            }
        }
    }
}
