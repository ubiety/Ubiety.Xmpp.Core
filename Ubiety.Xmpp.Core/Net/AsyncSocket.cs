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
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Infrastructure.Extensions;
using Ubiety.Xmpp.Core.Logging;

namespace Ubiety.Xmpp.Core.Net
{
    /// <summary>
    ///     An asynchronous socket for connecting to a server
    /// </summary>
    public class AsyncSocket : ISocket, IDisposable
    {
        private const int BufferSize = 4096;
        private readonly byte[] _buffer;
        private readonly IClient _client;
        private readonly UTF8Encoding _utf8 = new UTF8Encoding();
        private readonly ILog _logger;
        private Address _address;
        private Socket _socket;
        private Stream _stream;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AsyncSocket" /> class
        /// </summary>
        /// <param name="client">Client to use for the server connection</param>
        public AsyncSocket(IClient client)
        {
            _logger = Log.Get<AsyncSocket>();
            _client = client;
            _buffer = new byte[BufferSize];
            _logger.Log(LogLevel.Debug, "AsyncSocket created");
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _socket.Dispose();
        }

        /// <inheritdoc />
        public event EventHandler<DataEventArgs> Data;

        /// <inheritdoc />
        public event EventHandler Connection;

        /// <inheritdoc />
        public bool Connected { get; private set; }

        /// <inheritdoc />
        public void Connect()
        {
            _logger.Log(LogLevel.Debug, "Connecting to server");
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
                _socket.ConnectAsync(args);
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

        private static bool CertificateValidation(object sender, X509Certificate certificate, X509Chain chain,
            SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None;
        }

        private void ConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            _logger.Log(LogLevel.Debug, "Connection complete");
            var socket = e.ConnectSocket;
            Connected = true;
            OnConnection();

            _stream = new NetworkStream(socket);
            if (_client.UseSsl) StartSsl();

            _logger.Log(LogLevel.Debug, "Starting to read data");
            _stream.BeginRead(_buffer, 0, BufferSize, ReceiveCompleted, null);
        }

        private void StartSsl()
        {
            _logger.Log(LogLevel.Debug, "Starting SSL encryption");
            var secureStream = new SslStream(_stream, true, CertificateValidation);

            secureStream.AuthenticateAsClient(_address.Hostname, null, SslProtocols.Tls, false);

            if (secureStream.IsAuthenticated) _stream = secureStream;
        }

        private void ReceiveCompleted(IAsyncResult ar)
        {
            _stream.EndRead(ar);
            var message = _utf8.GetString(_buffer.TrimNullBytes());

            OnData(new DataEventArgs {Message = message});

            _buffer.Clear();

            _stream.BeginRead(_buffer, 0, BufferSize, ReceiveCompleted, null);
        }
    }
}