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

namespace Ubiety.Xmpp.Core.Common;

/// <summary>
/// Base abstract class for XMPP implementation that provides core functionality for XMPP communication.
/// Handles connection management, state transitions, and SASL authentication.
/// </summary>
/// <remarks>
/// This class implements IDisposable and manages the lifecycle of network connections and resources.
/// It coordinates between different components like the parser, socket, and various registries needed
/// for XMPP communication.
/// </remarks>
public abstract class XmppBase : IDisposable
{
    private readonly ILog _logger;
    private readonly AsyncClientSocket _clientSocket;
    private bool _disposedValue; // To detect redundant calls

    /// <summary>
    /// Initializes a new instance of the <see cref="XmppBase"/> class.
    /// Sets up logging and initializes basic components needed for XMPP communication.
    /// </summary>
    protected XmppBase()
    {
        _logger = Log.Get<XmppBase>();
        _logger.Log(LogLevel.Debug, $"{GetType()} created");
    }

    /// <summary>
    /// Event that is raised when a stream error occurs during XMPP communication.
    /// </summary>
    /// <remarks>
    /// Subscribers receive an <see cref="ErrorEventArgs"/> containing error details.
    /// </remarks>
    public event EventHandler<ErrorEventArgs> Error;

    /// <summary>
    /// Gets or sets the port number used for XMPP communication.
    /// Defaults to the standard XMPP port 5222.
    /// </summary>
    public int Port { get; set; } = 5222;

    /// <summary>
    /// Gets a value indicating whether SSL/TLS encryption should be used for the connection.
    /// This value can only be set during initialization.
    /// </summary>
    public bool UseSsl { get; internal init; }

    /// <summary>
    /// Gets a value indicating whether IPv6 should be used for network communication.
    /// This value can only be set during initialization.
    /// </summary>
    public bool UseIPv6 { get; internal init; }

    /// <summary>
    /// Gets or sets the current state of the XMPP connection.
    /// States control the behavior and progression of the XMPP session.
    /// </summary>
    public IState State { get; set; }

    /// <summary>
    /// Gets the registry for XMPP tags, which manages the creation and lookup of XML elements.
    /// This value can only be set during initialization.
    /// </summary>
    public TagRegistry TagRegistry { get; internal init; }

    /// <summary>
    /// Gets the registry for SASL authentication mechanisms.
    /// This value can only be set during initialization.
    /// </summary>
    public SaslRegistry SaslRegistry { get; internal init; }

    /// <summary>
    /// Gets the client socket used for network communication.
    /// Handles the low-level network operations for XMPP communication.
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
    /// Gets or sets the SASL processor used for authentication during the current session.
    /// </summary>
    public SaslProcessor SaslProcessor { get; set; }

    /// <summary>
    /// Gets the XMPP protocol parser that handles incoming XML streams.
    /// This value can only be set during initialization.
    /// </summary>
    protected Parser Parser { get; init; }

    /// <summary>
    /// Releases all resources used by the <see cref="XmppBase"/> instance.
    /// </summary>
    public void Dispose()
    {
        // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Handles incoming XMPP tags from the parser and manages state transitions.
    /// </summary>
    /// <param name="sender">The source of the tag event.</param>
    /// <param name="e">Event arguments containing the parsed XMPP tag.</param>
    /// <remarks>
    /// This method checks for stream errors and executes the appropriate state actions
    /// based on the received tag.
    /// </remarks>
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
    /// Releases the unmanaged resources used by the <see cref="XmppBase"/> and optionally
    /// releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources;
    /// false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        _logger.Log(LogLevel.Debug, "Dispose(bool) called");
        if (_disposedValue)
        {
            return;
        }

        if (disposing)
        {
            _logger.Log(LogLevel.Debug, $"Disposing {_clientSocket.GetType()}");
            _clientSocket.Dispose();
        }

        _disposedValue = true;
    }

    /// <summary>
    /// Raises the Error event with the specified sender and error arguments.
    /// </summary>
    /// <param name="sender">The source of the error.</param>
    /// <param name="e">Error event arguments containing error details.</param>
    private void OnError(object sender, ErrorEventArgs e)
    {
        Error?.Invoke(sender, e);
    }

    /// <summary>
    /// Handles the connection event from the client socket.
    /// Initializes the parser and transitions to the connected state.
    /// </summary>
    /// <param name="sender">The source of the connection event.</param>
    /// <param name="e">Event arguments.</param>
    private void Socket_Connection(object sender, EventArgs e)
    {
        _logger.Log(LogLevel.Debug, "Setting connection state");
        Parser.Start();
        State = new ConnectedState();
        State.Execute(this);
    }
}
