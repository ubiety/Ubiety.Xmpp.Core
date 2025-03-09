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
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core
{
    /// <summary>
    /// The XmppClient class is responsible for establishing and managing client connections to an XMPP server.
    /// Implements core functionality for XMPP communication, including authentication and state transitions, extending XmppBase and adhering to the IClient interface.
    /// </summary>
    public class XmppClient : XmppBase, IClient
    {
        private readonly ILog _logger;

        /// <summary>
        ///     Initializes a new instance of the <see cref="XmppClient" /> class.
        /// </summary>
        internal XmppClient()
        {
            _logger = Log.Get<XmppClient>();
            _logger.Log(LogLevel.Debug, $"{typeof(XmppClient)} created");
            ClientSocket = new AsyncClientSocket(this);
            Parser = new Parser(this);
            Parser.Tag += Parser_Tag;
        }

        /// <summary>
        /// Event triggered when a new stanza is received from the server.
        /// </summary>
        public event EventHandler<TagEventArgs> Stanza;

        /// <summary>
        /// Gets or sets the Jabber Identifier (JID) of the client.
        /// </summary>
        public Jid Id { get; set; }

        /// <summary>
        /// Gets or sets the password used for authenticating the client with the XMPP server.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets a value indicating whether the client is authenticated with the server.
        /// This property is set internally during the authentication process.
        /// </summary>
        public bool Authenticated { get; internal set; }

        /// <summary>
        /// Gets or sets the resource identifier associated with the XMPP client connection.
        /// This value is used to uniquely identify a connection to the XMPP server, particularly when multiple
        /// connections are established using the same JID.
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// Establishes a connection to the XMPP server using the provided JID and password.
        /// </summary>
        /// <param name="jid">The JID (Jabber Identifier) to use for the connection.</param>
        /// <param name="password">The password associated with the JID.</param>
        /// <exception cref="ArgumentNullException">Thrown when the provided JID is null.</exception>
        public void Connect(Jid jid, string password)
        {
            _logger.Log(LogLevel.Debug, "Connect(Jid, string) called");
            ArgumentNullException.ThrowIfNull(jid);

            _logger.Log(LogLevel.Debug, $"Connecting to server for {jid}");
            Id = jid;
            Password = password;
            State = new ConnectingState();
            State.Execute(this);
        }

        /// <summary>
        /// Triggers the stanza event with the supplied tag.
        /// </summary>
        /// <param name="tag">Tag to send as part of the event.</param>
        internal void OnStanza(Tag tag)
        {
            Stanza?.Invoke(this, new TagEventArgs { Tag = tag });
        }
    }
}
