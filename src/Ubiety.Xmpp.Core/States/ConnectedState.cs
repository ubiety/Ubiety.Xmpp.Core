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
using Ubiety.Xmpp.Core.Tags.Stream;

namespace Ubiety.Xmpp.Core.States
{
    /// <summary>
    /// Represents the connected state of the XMPP client.
    /// </summary>
    /// <remarks>
    /// This state is responsible for initiating the XMPP client's XML stream by starting the XMPP communication
    /// process and transitioning to the next state.
    /// </remarks>
    public class ConnectedState : IState
    {
        /// <summary>
        /// Executes the operations corresponding to the connected state of the XMPP client.
        /// </summary>
        /// <param name="xmpp">
        /// The XMPP client instance currently in use. This parameter must derive from the <see cref="XmppBase"/> class.
        /// </param>
        /// <param name="tag">
        /// An optional XMPP tag representing part of the communication stream. Defaults to null if not provided.
        /// </param>
        public void Execute(XmppBase xmpp, Tag tag = null)
        {
            if (xmpp is XmppClient client)
            {
                var stream = xmpp.Registry.GetTag<Stream>(Stream.XmlName);
                stream.Version = "1.0";
                stream.To = client.Id.Server;
                stream.Namespace = Namespaces.Client;

                client.ClientSocket.Send(stream.StartTag);
                client.ClientSocket.SetReadClear();

                xmpp.State = new StreamFeaturesState();
            }
        }
    }
}
