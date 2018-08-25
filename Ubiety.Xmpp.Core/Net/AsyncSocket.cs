// <copyright file="AsyncSocket.cs" company="Dieter Lunn">
// Copyright (c) Dieter Lunn. All rights reserved.
// </copyright>

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
using Ubiety.Xmpp.Core.Interfaces;

namespace Ubiety.Xmpp.Core.Net
{
    /// <summary>
    ///     An asynchronous socket for connecting to a server
    /// </summary>
    public class AsyncSocket : ISocket, IDisposable
    {
        private const int BufferSize = 4096;
        private readonly byte[] _buffer;
        private readonly UTF8Encoding _utf8 = new UTF8Encoding();
        private readonly IClient _client;
        private Address _address;
        private Socket _socket;
        private Stream _stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSocket"/> class
        /// </summary>
        /// <param name="client">Client to use for the server connection</param>
        public AsyncSocket(IClient client)
        {
            _client = client;
            _buffer = new byte[BufferSize];
        }

        /// <inheritdoc />
        public event EventHandler<DataEventArgs> Data;

        /// <inheritdoc/>
        public event EventHandler Connection;

        /// <inheritdoc/>
        public bool Connected { get; private set; }

        /// <inheritdoc />
        public void Dispose()
        {
            _socket.Dispose();
        }

        /// <inheritdoc />
        public void Connect()
        {
            _address = new Address(_client);
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
                Console.WriteLine(e);
                throw;
            }
        }

        /// <inheritdoc />
        public void Disconnect()
        {
            Connected = false;
            _stream.Close();
            _socket.Shutdown(SocketShutdown.Both);
            _socket.Disconnect(true);
        }

        /// <summary>
        /// Raise the data event with the specified arguments
        /// </summary>
        /// <param name="e">Data event arguments</param>
        protected virtual void OnData(DataEventArgs e)
        {
            Data?.Invoke(this, e);
        }

        /// <summary>
        /// Raise the connection event
        /// </summary>
        protected virtual void OnConnection()
        {
            Connection?.Invoke(this, new EventArgs());
        }

        private static bool CertificateValidation(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return sslPolicyErrors == SslPolicyErrors.None;
        }

        private void ConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            var socket = e.ConnectSocket;
            Connected = true;
            OnConnection();

            _stream = new NetworkStream(socket);
            if (_client.UseSsl)
            {
                StartSsl();
            }

            _stream.BeginRead(_buffer, 0, BufferSize, ReceiveCompleted, null);
        }

        private void StartSsl()
        {
            var secureStream = new SslStream(_stream, true, CertificateValidation);

            secureStream.AuthenticateAsClient(_address.Hostname, null, SslProtocols.Tls, false);

            if (secureStream.IsAuthenticated)
            {
                _stream = secureStream;
            }
        }

        private void ReceiveCompleted(IAsyncResult ar)
        {
            _stream.EndRead(ar);
            var message = _utf8.GetString(_buffer.TrimNullBytes());

            OnData(new DataEventArgs { Message = message });

            _buffer.Clear();

            _stream.BeginRead(_buffer, 0, BufferSize, ReceiveCompleted, null);
        }
    }
}