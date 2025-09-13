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

using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Ubiety.Xmpp.Core.Client;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core.Net;

/// <summary>
///     An asynchronous socket for connecting to an XMPP server.
/// </summary>
public class AsyncClientSocket : ISocket, IDisposable
{
    /// <summary>
    ///     The size of the buffer used for reading data from the stream.
    /// </summary>
    private const int BufferSize = 4 * 1024;

    /// <summary>
    ///     The XMPP client instance associated with this socket.
    /// </summary>
    private readonly IClient _client;

    /// <summary>
    ///     Logger instance for logging socket operations.
    /// </summary>
    private readonly ILog _logger = Log.Get<AsyncClientSocket>();

    /// <summary>
    ///     Event used to signal when the socket is ready to read data.
    /// </summary>
    private readonly AutoResetEvent _resetEvent;
    private readonly UTF8Encoding _utf8 = new();
    private Address _address;

    /// <summary>
    ///     The underlying network socket.
    /// </summary>
    private Socket _socket;

    /// <summary>
    ///     The stream used for network communication.
    /// </summary>
    private Stream _stream;

    /// <summary>
    ///     Initializes a new instance of the <see cref="AsyncClientSocket" /> class.
    /// </summary>
    /// <param name="client">Client to use for the server connection.</param>
    public AsyncClientSocket(IClient client)
    {
        _client = client;
        _logger.Log(LogLevel.Debug, $"{typeof(AsyncClientSocket)} created");
        _resetEvent = new AutoResetEvent(false);
    }

    /// <summary>
    ///     Occurs when the socket has successfully connected to the server.
    /// </summary>
    public event EventHandler Connection;

    /// <summary>
    ///     Occurs when data is received from the server.
    /// </summary>
    public event EventHandler<DataEventArgs> Data;

    /// <summary>
    ///     Gets a value indicating whether the socket is connected to the server.
    /// </summary>
    public bool Connected { get; private set; }

    /// <summary>
    ///     Gets a value indicating whether the socket is secured with SSL/TLS.
    /// </summary>
    public bool Secure { get; private set; }

    /// <summary>
    ///     Releases all resources used by the <see cref="AsyncClientSocket"/>.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    ///     Connects the client to the XMPP server using the specified JID.
    /// </summary>
    /// <param name="jid">The Jabber ID (JID) to use for the connection.</param>
    public void Connect(Jid jid)
    {
        _logger.Log(LogLevel.Debug, "Connect(Jid) called");
        _client.Id = jid;
        _address = new Address(_client);
        _logger.Log(LogLevel.Debug, "Creating socket");
        _socket = _address.IsIPv6
            ? new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp)
            : new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

        var args = new SocketAsyncEventArgs();
        args.Completed += ConnectCompleted;
        args.RemoteEndPoint = new IPEndPoint(_address.NextIpAddress(), _client.Port);

        try
        {
            _logger.Log(LogLevel.Debug, "Starting asynchronous connection");

            var completed = _socket.ConnectAsync(args);

            if (!completed)
            {
                _logger.Log(LogLevel.Debug, "Connect completed synchronously");
                ConnectCompleted(this, args);
            }
        }
        catch (SocketException e)
        {
            _logger.Log(LogLevel.Error, e, $"Error connecting to the server with error code {e.ErrorCode}");
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    ///     Disconnects the client from the XMPP server and releases associated resources.
    /// </summary>
    public void Disconnect()
    {
        _logger.Log(LogLevel.Debug, "Disconnect() called");
        Connected = false;
        _stream?.Close();
        _socket?.Shutdown(SocketShutdown.Both);
        _socket?.Disconnect(true);
    }

    /// <summary>
    ///     Sends a string message to the server.
    /// </summary>
    /// <param name="message">The message to send to the server.</param>
    public void Send(string message)
    {
        SendAsync(message).GetAwaiter().GetResult();
    }

    /// <summary>
    ///     Asynchronously sends a message to the server.
    /// </summary>
    /// <param name="message">The message to send.</param>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous send operation.</returns>
    public async Task SendAsync(string message, CancellationToken cancellationToken = default)
    {
        if (!Connected || _stream is null)
        {
            return;
        }

        _logger.Log(LogLevel.Debug, $"Sending message: {message}");

        var bytes = _utf8.GetBytes(message);
        _stream?.WriteAsync(bytes, 0, bytes.Length);
    }

    /// <summary>
    ///     Sends a tag to the server synchronously.
    /// </summary>
    /// <param name="tag">The <see cref="Tag"/> to send.</param>
    public void Send(Tag tag)
    {
        Send(tag.ToString());
    }

    /// <summary>
    ///     Starts an SSL/TLS session on the current connection.
    /// </summary>
    public void StartSsl()
    {
        _logger.Log(LogLevel.Debug, "StartSsl() called");
        var secureStream = new SslStream(_stream!, true, CertificateValidation);

        _logger.Log(LogLevel.Debug, "Authenticating as client...");
        secureStream.AuthenticateAsClient(_address!.Hostname);
        _logger.Log(LogLevel.Debug, $"Using SSL protocol version: {secureStream.SslProtocol}");

        if (secureStream.IsAuthenticated)
        {
            _logger.Log(LogLevel.Debug, "Stream is encrypted");
            Secure = true;
            _stream = secureStream;
            _client.State?.Execute((XmppClient)_client);
        }
    }

    /// <summary>
    ///     Signals that the socket is ready to read data.
    /// </summary>
    public void SetReadClear()
    {
        _logger.Log(LogLevel.Debug, "SetReadClear() called");
        _resetEvent.Set();
    }

    /// <summary>
    ///     Releases the unmanaged resources used by the <see cref="AsyncClientSocket"/> and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    protected virtual void Dispose(bool disposing)
    {
        _logger.Log(LogLevel.Debug, "Dispose(bool) called");
        if (disposing)
        {
            _logger.Log(LogLevel.Debug, $"Disposing {_socket?.GetType()}");
            _socket?.Dispose();
            _stream?.Dispose();
            _resetEvent.Dispose();
        }
    }

    /// <summary>
    ///     Raises the <see cref="Data"/> event with the specified arguments.
    /// </summary>
    /// <param name="e">The data event arguments.</param>
    private void OnData(DataEventArgs e)
    {
        _logger.Log(LogLevel.Debug, "OnData(DataEventArgs) called");
        Data?.Invoke(this, e);
    }

    /// <summary>
    ///     Raises the <see cref="Connection"/> event.
    /// </summary>
    private void OnConnection()
    {
        _logger.Log(LogLevel.Debug, "OnConnection() called");
        Connection?.Invoke(this, EventArgs.Empty);
    }

    /// <summary>
    ///     Validates the SSL certificate during authentication.
    /// </summary>
    /// <param name="sender">The sender of the validation event.</param>
    /// <param name="certificate">The certificate to validate.</param>
    /// <param name="chain">The certificate chain.</param>
    /// <param name="sslPolicyErrors">The SSL policy errors, if any.</param>
    /// <returns>true if the certificate is valid; otherwise, false.</returns>
    private bool CertificateValidation(
        object sender,
        X509Certificate certificate,
        X509Chain chain,
        SslPolicyErrors sslPolicyErrors)
    {
        if (sslPolicyErrors == SslPolicyErrors.None)
        {
            return true;
        }

        if (chain != null && chain.ChainStatus.Length == 1 &&
            (sslPolicyErrors == SslPolicyErrors.RemoteCertificateChainErrors ||
             certificate.Subject == certificate.Issuer) &&
            chain.ChainStatus[0].Status == X509ChainStatusFlags.UntrustedRoot)
        {
            return true;
        }

        _logger.Log(LogLevel.Debug, certificate.ToString());
        _logger.Log(LogLevel.Error, $"Policy errors: {sslPolicyErrors}");

        return false;
    }

    /// <summary>
    ///     Handles the completion of the asynchronous connect operation.
    /// </summary>
    /// <param name="sender">The sender of the event.</param>
    /// <param name="e">The socket async event arguments.</param>
    private void ConnectCompleted(object sender, SocketAsyncEventArgs e)
    {
        _logger.Log(LogLevel.Debug, "ConnectCompleted(object, SocketEventArgs) called");

        if (e.SocketError != SocketError.Success)
        {
            _logger.Log(LogLevel.Error, $"Error connecting to server: {e.SocketError}");
            return;
        }

        var socket = e.ConnectSocket;
        _stream = new NetworkStream(socket ?? throw new InvalidOperationException());

        Connected = true;
        OnConnection();

        BeginReadAsync().ConfigureAwait(false);
    }

    /// <summary>
    ///     Begins reading data asynchronously from the server.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous read operation.</returns>
    private async Task BeginReadAsync()
    {
        _logger.Log(LogLevel.Debug, "BeginReadAsync() called");
        while (Connected)
        {
            var message = await ReadDataAsync().ConfigureAwait(false);
            _logger.Log(LogLevel.Debug, $"Received message: {message}");
            OnData(new DataEventArgs { Message = message });
        }
    }

    /// <summary>
    ///     Reads data from the stream asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous read operation, with the received message as a string.</returns>
    private Task<string> ReadData()
    {
        return ReadDataAsync();
    }

    /// <summary>
    ///     Asynchronously reads data from the stream.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task{TResult}"/> representing the asynchronous read operation, with the received message as a string.</returns>
    private async Task<string> ReadDataAsync(CancellationToken cancellationToken = default)
    {
        _resetEvent.WaitOne();
        var buffer = new byte[BufferSize];
        if (_stream is null)
        {
            return string.Empty;
        }

        var received = await _stream.ReadAsync(buffer, 0, BufferSize, cancellationToken).ConfigureAwait(false);
        Array.Resize(ref buffer, received);
        var message = _utf8.GetString(buffer);
        return message;
    }
}
