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

using System.Xml.Linq;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Tags;
using Ubiety.Xmpp.Core.Tags.Binding;
using Ubiety.Xmpp.Core.Tags.Client;

namespace Ubiety.Xmpp.Core.States
{
    /// <summary>
    ///     Resource binding state.
    /// </summary>
    public class BindingState : IState
    {
        /// <inheritdoc />
        public void Execute(XmppBase xmpp, Tag tag = null)
        {
            var client = xmpp as XmppClient;

            if (tag is null)
            {
                var bind = xmpp.Registry.GetTag<Bind>(XName.Get("bind", Namespaces.Bind));
                var iq = xmpp.Registry.GetTag<Iq>(XName.Get("iq", Namespaces.Client));

                if (!string.IsNullOrEmpty(client.Resource))
                {
                }

                iq.IqType = IqType.Set;
                iq.Add(bind);

                xmpp.ClientSocket.SetReadClear();
                xmpp.ClientSocket.Send(iq);
            }
            else
            {
            }
        }
    }
}
