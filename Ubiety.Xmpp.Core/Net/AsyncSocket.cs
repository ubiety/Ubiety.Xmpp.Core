using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Ubiety.Xmpp.Core.Infrastructure.Extensions;
using Ubiety.Xmpp.Core.Interfaces;

namespace Ubiety.Xmpp.Core.Net
{
    /// <summary>
    ///     An asynchronous socket for connecting to a server
    /// </summary>
    public class AsyncSocket : ISocket, IDisposable
    {
        private readonly byte[] _buffer;
        private readonly UTF8Encoding _utf8 = new UTF8Encoding();
        private Address _address;
        private Socket _socket;

        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncSocket"/> class
        /// </summary>
        public AsyncSocket()
        {
            _buffer = new byte[2048];
        }

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
            throw new NotImplementedException();
        }

        private void ConnectCompleted(object sender, SocketAsyncEventArgs e)
        {
            var socket = e.ConnectSocket;
            Connected = true;

            var args = new SocketAsyncEventArgs();
            args.Completed += ReceiveCompleted;
            args.SetBuffer(_buffer, 0, _buffer.Length);

            socket.ReceiveAsync(args);
        }

        private void ReceiveCompleted(object sender, SocketAsyncEventArgs e)
        {
            var buffer = e.Buffer.TrimNullBytes();
            var message = _utf8.GetString(buffer);
        }
    }
}