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

using System;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Infrastructure;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Net;
using Ubiety.Xmpp.Core.States;

namespace Ubiety.Xmpp.Core
{
    /// <summary>
    ///     Main XMPP client class
    /// </summary>
    public class XmppClient : XmppBase, IClient
    {
        private readonly ILog _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="XmppClient" /> class
        /// </summary>
        internal XmppClient()
        {
            _logger = Log.Get<XmppClient>();
            ClientSocket = new AsyncClientSocket(this);
            _logger.Log(LogLevel.Debug, "XmppClient created");
            Parser = new Parser(this);
            Parser.Tag += Parser_Tag;
        }

        /// <inheritdoc />
        public Jid Id { get; set; }

        /// <summary>
        ///     Gets or sets the user password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     Gets a value indicating whether the user is authenticated
        /// </summary>
        public bool Authenticated { get; internal set; }

        /// <inheritdoc />
        public void Connect(Jid jid, string password)
        {
            if (jid is null) throw new ArgumentNullException(nameof(jid));

            _logger.Log(LogLevel.Debug, $"Connecting to server for {jid}");
            Id = jid;
            Password = password;
            State = new ConnectingState();
            State.Execute(this);
        }
    }
}