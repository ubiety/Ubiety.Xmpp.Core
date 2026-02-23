using System;
using System.Collections.Concurrent;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ubiety.Xmpp.Core.Server
{
    /// <summary>
    /// Basic XMPP server implementation (RFC 6120/6121 skeleton).
    /// </summary>
    public class XmppServer : IDisposable
    {
        private readonly TcpListener _listener;
        private readonly ConcurrentDictionary<TcpClient, Task> _clients = new();
        private CancellationTokenSource _cts;

        public int Port { get; }

        public XmppServer(int port = 5222)
        {
            Port = port;
            _listener = new TcpListener(IPAddress.Any, port);
        }

        public void Start()
        {
            _cts = new CancellationTokenSource();
            _listener.Start();
            Task.Run(() => AcceptClientsAsync(_cts.Token));
        }

        public void Stop()
        {
            _cts?.Cancel();
            _listener.Stop();
        }

        private async Task AcceptClientsAsync(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    var client = await _listener.AcceptTcpClientAsync(token);
                    var task = HandleClientAsync(client, token);
                    _clients[client] = task;
                }
                catch (OperationCanceledException) { break; }
                catch (Exception) { /* Log error */ }
            }
        }

        private async Task HandleClientAsync(TcpClient client, CancellationToken token)
        {
            using (client)
            using (var stream = client.GetStream())
            {
                // Send initial stream header (RFC 6120 4.4.1)
                var response = "<?xml version='1.0'?><stream:stream from='localhost' id='12345' version='1.0' xmlns='jabber:client' xmlns:stream='http://etherx.jabber.org/streams'>";
                var buffer = Encoding.UTF8.GetBytes(response);
                await stream.WriteAsync(buffer, 0, buffer.Length, token);

                // TODO: Read client stream, negotiate features, authenticate, handle stanzas
            }
        }
    }
}
