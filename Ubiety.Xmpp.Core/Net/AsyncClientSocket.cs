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
using System.Security.Authentication;
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
    ///     An asynchronous socket for connecting to an XMPP server
    /// </summary>
    public class AsyncClientSocket : ISocket, IDisposable
    {
        private const int BufferSize = 4 * 1024;
        private readonly IClient _client;
        private readonly ILog _logger = Log.Get<AsyncClientSocket>();
        private readonly AutoResetEvent _resetEvent;
        private readonly UTF8Encoding _utf8 = new UTF8Encoding();
        private Address _address;
        private Socket _socket;
        private Stream _stream;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncClientSocket" /> class
        /// </summary>
        /// <param name="client">Client to use for the server connection</param>
        public AsyncClientSocket(IClient client)
        {
            _client = client;
            _logger.Log(LogLevel.Debug, "AsyncClientSocket created");
            _resetEvent = new AutoResetEvent(false);
        }

        /// <inheritdoc />
        public event EventHandler Connection;

        /// <inheritdoc />
        public event EventHandler<DataEventArgs> Data;

        /// <inheritdoc />
        public bool Connected { get; private set; }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <inheritdoc />
        public void Connect(Jid jid)
        {
            _logger.Log(LogLevel.Debug, "Connecting to server");
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
                _logger.Log(LogLevel.Debug, "Starting async connection");

                if (!_socket.ConnectAsync(args))
                {
                    ConnectCompleted(this, args);
                }
            }
            catch (SocketException e)
            {
                _logger.Log(LogLevel.Error, e, "Error connecting to the server");
                Console.WriteLine(e);
                throw;
            }
        }

        /// <inheritdoc />
        public void Disconnect()
        {
            _logger.Log(LogLevel.Debug, "Disconnecting from the server");
            Connected = false;
            _stream.Close();
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Disconnect(true);
        }

        /// <inheritdoc />
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
        ///     Sends a tag to the server
        /// </summary>
        /// <param name="tag"><see cref="Tag" /> to send</param>
        public void Send(Tag tag)
        {
            Send(tag.ToString());
        }

        /// <summary>
        ///     Starts SSL/TLS connection
        /// </summary>
        public void StartSsl()
        {
            _logger.Log(LogLevel.Debug, "Starting SSL encryption");
            var secureStream = new SslStream(_stream, true, CertificateValidation);

            _logger.Log(LogLevel.Debug, "Authenticating as client...");
            secureStream.AuthenticateAsClient(_address.Hostname, null, SslProtocols.Tls12 | SslProtocols.Tls11, false);
            _logger.Log(LogLevel.Debug, $"Using SSL protocol version: {secureStream.SslProtocol.ToString()}");

            if (secureStream.IsAuthenticated)
            {
                _logger.Log(LogLevel.Debug, "Stream is encrypted");
                _stream = secureStream;
                _client.State.Execute((XmppClient)_client);
            }
        }

        /// <summary>
        ///     Set that we are clear to read data
        /// </summary>
        public void SetReadClear()
        {
            _logger.Log(LogLevel.Debug, "Setting read clear to true");
            _resetEvent.Set();
        }

        /// <summary>
        ///     Raise the data event with the specified arguments
        /// </summary>
        /// <param name="e">Data event arguments</param>
        protected virtual void OnData(DataEventArgs e)
        {
            _logger.Log(LogLevel.Debug, "Firing data event");
            Data?.Invoke(this, e);
        }

        /// <summary>
        ///     Raise the connection event
        /// </summary>
        protected virtual void OnConnection()
        {
            _logger.Log(LogLevel.Debug, "Firing connection event");
            Connection?.Invoke(this, new EventArgs());
        }

        /// <summary>
        ///     Dispose of class resources
        /// </summary>
        /// <param name="disposing">Are we disposing from a direct call</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _socket?.Dispose();
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
            _logger.Log(LogLevel.Debug, "Connection complete");

            if (e.SocketError != SocketError.Success)
            {
                _logger.Log(LogLevel.Error, $"Error connecting to server: {e.SocketError}");
                return;
            }

            var socket = e.ConnectSocket;
            _stream = new NetworkStream(socket);

            Connected = true;
            OnConnection();

            _logger.Log(LogLevel.Debug, "Starting to read data");
            BeginRead();
        }

        private void BeginRead()
        {
            while (Connected)
            {
                var message = ReadData();
                _logger.Log(LogLevel.Debug, $"Received message: {message.Result}");
                OnData(new DataEventArgs { Message = message.Result });
            }
        }

        private Task<string> ReadData()
        {
            _resetEvent.WaitOne();
            var buffer = new byte[BufferSize];
            var received = _stream.ReadAsync(buffer, 0, BufferSize);

            var task = received.ContinueWith(getString =>
            {
                Array.Resize(ref buffer, received.Result);
                var message = _utf8.GetString(buffer);
                return message;
            });

            return task;
        }
    }
}