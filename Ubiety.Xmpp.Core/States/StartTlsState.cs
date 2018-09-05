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

using System.Xml.Linq;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Tags;
using Ubiety.Xmpp.Core.Tags.Tls;

namespace Ubiety.Xmpp.Core.States
{
    /// <inheritdoc />
    public class StartTlsState : IState
    {
        private static ILog _logger;

        static StartTlsState()
        {
            _logger = Log.Get<StartTlsState>();
        }

        /// <inheritdoc />
        public void Execute(XmppBase xmpp, Tag tag = null)
        {
            if (tag is Proceed)
            {
                _logger.Log(LogLevel.Debug, "Clear to start SSL/TLS connection");
                xmpp.State = new ConnectedState();
                xmpp.ClientSocket.StartSsl();
                return;
            }

            _logger.Log(LogLevel.Debug, "Sending starttls");
            var starttls = xmpp.Registry.GetTag<StartTls>(XName.Get("starttls", Namespaces.Tls));
            xmpp.ClientSocket.Send(starttls);
        }
    }
}
