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

namespace Ubiety.Xmpp.Core
{
    /// <summary>
    ///     Main XMPP client class
    /// </summary>
    public class XmppClient : IClient
    {
        private readonly ILog _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="XmppClient" /> class
        /// </summary>
        internal XmppClient()
        {
            _logger = Log.Get<XmppClient>();
            _logger.Log(LogLevel.Debug, "XmppClient created");
        }

        /// <inheritdoc />
        public Jid Id { get; set; }

        /// <inheritdoc />
        public int Port { get; set; }

        /// <inheritdoc />
        public bool UseSsl { get; internal set; }
    }
}