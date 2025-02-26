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
using System.Linq;
using Ubiety.Xmpp.Core.Infrastructure;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Net;
using Ubiety.Xmpp.Core.Registries;
using Ubiety.Xmpp.Core.Sasl;
using Ubiety.Xmpp.Core.States;
using Ubiety.Xmpp.Core.Tags.Stream;

namespace Ubiety.Xmpp.Core.Common
{
    /// <summary>
    /// Represents the base class for XMPP clients, providing core functionalities and common elements for maintaining XMPP communication.
    /// </summary>
    public abstract class XmppBase : IDisposable
    {
        private readonly ILog _logger;
        private readonly AsyncClientSocket _clientSocket;
        private bool _disposedValue; // To detect redundant calls

        /// <summary>
        ///     Initializes a new instance of the <see cref="XmppBase" /> class.
        /// </summary>
        protected XmppBase()
        {
            _logger = Log.Get<XmppBase>();
            _logger.Log(LogLevel.Debug, $"{GetType()} created");
        }

        /// <summary>
        /// Occurs when an error is encountered in the XMPP client.
        /// </summary>
        public event EventHandler<ErrorEventArgs> Error;

        /// <summary>
        /// Gets or sets the port number used for XMPP communication.
        /// Default value is 5222.
        /// </summary>
        public int Port { get; set; } = 5222;

        /// <summary>
        /// Gets a value indicating whether SSL/TLS should be used for securing the XMPP connection.
        /// </summary>
        public bool UseSsl { get; internal init; }

        /// <summary>
        /// Gets a value indicating whether IPv6 is used for communication in the XMPP client.
        /// </summary>
        public bool UseIPv6 { get; internal init; }

        /// <summary>
        /// Gets or sets the current state of the XMPP client, managing the execution of state-specific logic.
        /// </summary>
        public IState State { get; set; }

        /// <summary>
        /// Gets the registry for managing and retrieving XMPP protocol tags.
        /// </summary>
        public TagRegistry Registry { get; internal init; }

        /// <summary>
        /// Gets the asynchronous client socket used for communication in the XMPP client.
        /// Provides mechanisms for sending and receiving data over a network connection.
        /// </summary>
        public AsyncClientSocket ClientSocket
        {
            get => _clientSocket;
            protected init
            {
                _clientSocket = value;
                _clientSocket.Connection += Socket_Connection;
            }
        }

        /// <summary>
        /// Gets or sets the SASL processor used for managing and handling the
        /// authentication mechanisms in the XMPP communication.
        /// </summary>
        public SaslProcessor SaslProcessor { get; set; }

        /// <summary>
        /// Gets the XMPP protocol parser used for processing incoming XML tags and managing protocol state transitions.
        /// </summary>
        protected Parser Parser { get; init; }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="XmppBase"/> class.
        /// </summary>
        /// <remarks>
        /// This method ensures the proper cleanup of managed and unmanaged resources.
        /// It suppresses finalization for the object to optimize garbage collection performance.
        /// </remarks>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Handles the tag parsed event and executes the current state or logs an error if one is present.
        /// </summary>
        /// <param name="sender">The object sending the event.</param>
        /// <param name="e">The arguments containing the tag from the event.</param>
        protected void Parser_Tag(object sender, TagEventArgs e)
        {
            if (e.Tag is Stream stream && stream.Errors.Any())
            {
                OnError(
                    this,
                    new ErrorEventArgs { Message = "Error occured", StreamError = stream.Errors.FirstOrDefault() });
                Parser.Stop();
                State = new DisconnectState();
            }

            _logger.Log(LogLevel.Debug, "Received a tag. Executing current state");
            State.Execute(this, e.Tag);
        }

        /// <summary>
        /// Releases all resources used by the current instance of the <see cref="XmppBase" /> class.
        /// </summary>
        /// <param name="disposing">Dispose managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            _logger.Log(LogLevel.Debug, "Dispose(bool) called");
            if (!_disposedValue)
            {
                if (disposing)
                {
                    _logger.Log(LogLevel.Debug, $"Disposing {_clientSocket.GetType()}");
                    _clientSocket.Dispose();
                }

                _disposedValue = true;
            }
        }

        private void OnError(object sender, ErrorEventArgs e)
        {
            Error?.Invoke(sender, e);
        }

        private void Socket_Connection(object sender, EventArgs e)
        {
            _logger.Log(LogLevel.Debug, "Setting connection state");
            Parser.Start();
            State = new ConnectedState();
            State.Execute(this);
        }
    }
}
