// <copyright file="AsyncSocket.cs" company="Dieter Lunn">
// Copyright (c) Dieter Lunn. All rights reserved.
// </copyright>

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
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
        private Address _address;
        private Socket _socket;
        private Stream _stream;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSocket"/> class
        /// </summary>
        public AsyncSocket()
        {
            _buffer = new byte[BufferSize];
        }

        /// <inheritdoc />
        public event EventHandler<DataEventArgs> Data;

        /// <summary>
        /// Gets a value indicating whether the socket is connected
        /// </summary>
        public bool Connected { get; private set; }

        /// <inheritdoc />
        public void Dispose()
        {
            _socket.Dispose();
        }

        /// <inheritdoc />
        public void Connect(IClient client)
        {
            _address = new Address(client);
            _socket = _address.IsIPv6
                ? new Socket(AddressFamily.InterNetworkV6, SocketType.Stream, ProtocolType.Tcp)
                : new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            var args = new SocketAsyncEventArgs();
            args.Completed += ConnectCompleted;
            args.RemoteEndPoint = new IPEndPoint(_address.NextIpAddress(), client.Port);

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

        private void ConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            var socket = e.ConnectSocket;
            Connected = true;

            _stream = new NetworkStream(socket);
            _stream.BeginRead(_buffer, 0, BufferSize, ReceiveCompleted, null);
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