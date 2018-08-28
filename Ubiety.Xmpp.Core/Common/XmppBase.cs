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
using Ubiety.Xmpp.Core.Infrastructure;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Net;
using Ubiety.Xmpp.Core.Registries;
using Ubiety.Xmpp.Core.States;

namespace Ubiety.Xmpp.Core.Common
{
    /// <summary>
    ///     Base XMPP implementation
    /// </summary>
    public abstract class XmppBase
    {
        private readonly ILog _logger;
        private AsyncClientSocket _clientSocket;

        /// <summary>
        ///     Initializes a new instance of the <see cref="XmppBase" /> class
        /// </summary>
        protected XmppBase()
        {
            _logger = Log.Get<XmppBase>();
            _logger.Log(LogLevel.Debug, "XmppBase created");
        }

        /// <inheritdoc cref="IClient" />
        public int Port { get; set; }

        /// <inheritdoc cref="IClient" />
        public bool UseSsl { get; internal set; }

        /// <inheritdoc cref="IClient" />
        public bool UseIPv6 { get; internal set; }

        /// <summary>
        ///     Gets or sets the current state
        /// </summary>
        public IState State { get; set; }

        /// <summary>
        ///     Gets the tag registry
        /// </summary>
        public TagRegistry Registry { get; internal set; }

        /// <summary>
        ///     Gets or sets the client socket
        /// </summary>
        public AsyncClientSocket ClientSocket
        {
            get => _clientSocket;
            protected set
            {
                _clientSocket = value;
                _clientSocket.Connection += _socket_Connection;
            }
        }

        /// <summary>
        ///     XMPP protocol parser
        /// </summary>
        protected Parser Parser { get; set; }

        private void _socket_Connection(object sender, EventArgs e)
        {
            _logger.Log(LogLevel.Debug, "Setting connection state");
            Parser.Start();
            State = new ConnectedState();
            State.Execute(this);
        }

        protected void Parser_Tag(object sender, TagEventArgs e)
        {
            State.Execute(this, e.Tag);
        }
    }
}