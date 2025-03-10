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
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core.Net
{
    /// <summary>
    /// Represents an asynchronous client socket for handling XMPP server connections.
    /// </summary>
    public sealed class AsyncClientSocket : ISocket, IDisposable
    {
        private const int BufferSize = 4 * 1024;
        private readonly IClient _client;
        private readonly ILog _logger = Log.Get<AsyncClientSocket>();
        private readonly AutoResetEvent _resetEvent;
        private readonly UTF8Encoding _utf8 = new ();
        private Address _address;
        private Socket _socket;
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
        /// Event triggered upon establishing a connection with the server.
        /// </summary>
        public event EventHandler Connection;

        /// <summary>
        /// Event triggered when data is received from the server.
        /// </summary>
        public event EventHandler<DataEventArgs> Data;

        /// <summary>
        /// Gets a value indicating whether the socket connection to the XMPP server is currently active.
        /// </summary>
        public bool Connected { get; private set; }

        /// <summary>
        /// Gets a value indicating whether the socket connection is secure.
        /// </summary>
        public bool Secure { get; private set; }

        /// <summary>
        /// Releases the resources used by the <see cref="AsyncClientSocket" /> instance.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        /// Establishes a connection to the server using the provided JID.
        /// </summary>
        /// <param name="jid">The Jabber Identifier (JID) to use for connection.</param>
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
        /// Disconnects the client socket from the server and releases the associated resources.
        /// </summary>
        public void Disconnect()
        {
            _logger.Log(LogLevel.Debug, "Disconnect() called");
            Connected = false;
            _stream.Close();
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Disconnect(true);
        }

        /// <summary>
        /// Sends a message to the connected XMPP server.
        /// </summary>
        /// <param name="message">The message to send to the server.</param>
        public void Send(string message)
        {
            if (!Connected)
            {
                return;
            }

            _logger.Log(LogLevel.Debug, $"Sending message: {message}");

            var bytes = _utf8.GetBytes(message);
            _stream.WriteAsync(bytes, 0, bytes.Length);
        }

        /// <summary>
        /// Sends a specific <see cref="Tag" /> to the server.
        /// </summary>
        /// <param name="tag">The <see cref="Tag" /> to be sent.</param>
        public void Send(Tag tag)
        {
            Send(tag.ToString());
        }

        /// <summary>
        /// Initiates an SSL/TLS connection by wrapping the existing stream with an <see cref="SslStream" />
        /// and authenticating as the client.
        /// </summary>
        /// <remarks>
        /// This method transitions the connection to a secure, encrypted state using the SSL/TLS protocols.
        /// It validates the server's certificate and updates the internal stream to use the secure stream.
        /// </remarks>
        public void StartSsl()
        {
            _logger.Log(LogLevel.Debug, "StartSsl() called");
            var secureStream = new SslStream(_stream, true, CertificateValidation);

            _logger.Log(LogLevel.Debug, "Authenticating as client...");
            secureStream.AuthenticateAsClient(_address.Hostname);
            _logger.Log(LogLevel.Debug, $"Using SSL protocol version: {secureStream.SslProtocol}");

            if (secureStream.IsAuthenticated)
            {
                Secure = true;
                _logger.Log(LogLevel.Debug, "Stream is encrypted");
                _stream = secureStream;
                _client.State.Execute((XmppClient)_client);
            }
        }

        /// <summary>
        /// Clears the read state by setting the reset event for the socket.
        /// </summary>
        public void SetReadClear()
        {
            _logger.Log(LogLevel.Debug, "SetReadClear() called");
            _resetEvent.Set();
        }

        /// <summary>
        /// Invokes the data event handler when new data is received.
        /// </summary>
        /// <param name="e">Data event arguments containing the received message.</param>
        private void OnData(DataEventArgs e)
        {
            _logger.Log(LogLevel.Debug, "OnData(DataEventArgs) called");
            Data?.Invoke(this, e);
        }

        /// <summary>
        /// Invokes the Connection event to signal that a connection has been established.
        /// </summary>
        private void OnConnection()
        {
            _logger.Log(LogLevel.Debug, "OnConnection() called");
            Connection?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Disposes of the resources used by the <see cref="AsyncClientSocket"/> instance.
        /// </summary>
        /// <param name="disposing">Indicates whether the method was invoked directly or by the garbage collector.</param>
        private void Dispose(bool disposing)
        {
            _logger.Log(LogLevel.Debug, "Dispose(bool) called");
            if (disposing)
            {
                _logger.Log(LogLevel.Debug, $"Disposing {_socket.GetType()}");
                _socket?.Dispose();
                _stream.Dispose();
                _resetEvent.Dispose();
            }
        }

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

            if (chain.ChainStatus.Length == 1 &&
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

        private async Task BeginReadAsync()
        {
            _logger.Log(LogLevel.Debug, "BeginReadAsync() called");
            while (Connected)
            {
                var message = await ReadData();
                _logger.Log(LogLevel.Debug, $"Received message: {message}");
                OnData(new DataEventArgs { Message = message });
            }
        }

        private Task<string> ReadData()
        {
            _resetEvent.WaitOne();
            var buffer = new byte[BufferSize];
            var received = _stream.ReadAsync(buffer, 0, BufferSize);

            var task = received.ContinueWith(_ =>
            {
                Array.Resize(ref buffer, received.Result);
                var message = _utf8.GetString(buffer);
                return message;
            });

            return task;
        }
    }
}
