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
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.States;
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core.Infrastructure;

/// <summary>
///     Parses XMPP protocol messages and raises tag events for further processing.
///     Handles incoming data, manages parsing lifecycle, and ensures proper resource cleanup.
/// </summary>
public sealed class Parser : IDisposable
{
    private readonly System.Collections.Concurrent.ConcurrentQueue<string> _dataQueue;
    private readonly ILog _logger = Log.Get<Parser>();
    private readonly XmppBase _xmpp;
    private XmlNamespaceManager _namespaceManager;
    private CancellationTokenSource _cts;
    private bool _disposed;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Parser"/> class.
    /// </summary>
    /// <param name="xmpp">The <see cref="XmppBase"/> instance to associate with this parser.</param>
    public Parser(XmppBase xmpp)
    {
        _xmpp = xmpp;
        _dataQueue = new();
        _xmpp.ClientSocket.Data += ClientSocket_Data;
        _logger.Log(LogLevel.Debug, $"{typeof(Parser)} created");
    }

    /// <summary>
    ///     Finalizes an instance of the <see cref="Parser"/> class. Ensures unmanaged resources are released.
    /// </summary>
    ~Parser()
    {
        Dispose(false);
    }

    /// <summary>
    ///     Occurs when a tag is parsed from the incoming XMPP data.
    /// </summary>
    public event EventHandler<TagEventArgs> Tag;

    private XmlNamespaceManager NamespaceManager
    {
        get
        {
            _namespaceManager ??= new XmlNamespaceManager(new NameTable());
            _namespaceManager.AddNamespace(string.Empty, Namespaces.Client);
            _namespaceManager.AddNamespace("stream", Namespaces.Stream);
            return _namespaceManager;
        }
    }

    /// <summary>
    ///     Starts the parsing process in a background task.
    ///     Incoming data will be processed and tag events raised until <see cref="Stop"/> or <see cref="Dispose()"/> is called.
    /// </summary>
    public void Start()
    {
        _logger.Log(LogLevel.Debug, "Start() called");
        _cts = new CancellationTokenSource();
        _ = ProcessQueueAsync(_cts.Token);
    }

    /// <summary>
    ///     Stops the parsing process and cancels the background task.
    ///     No further tag events will be raised after this is called.
    /// </summary>
    public void Stop()
    {
        _logger.Log(LogLevel.Debug, "Stop() called");
        _cts?.Cancel();
    }

    /// <summary>
    ///     Releases all resources used by the <see cref="Parser"/> class.
    ///     Cancels any running background tasks and detaches event handlers.
    /// </summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void OnTag(Tag tag)
    {
        _logger.Log(LogLevel.Debug, "OnTag(Tag) called");
        Tag?.Invoke(this, new TagEventArgs { Tag = tag });
    }

    private async Task ProcessQueueAsync(CancellationToken cancellationToken)
    {
        const string endStream = "</stream:stream>";

        while (true)
        {
            if (_xmpp.State is DisconnectedState || cancellationToken.IsCancellationRequested)
            {
                _logger.Log(LogLevel.Debug, "Disconnected or stopped");
                break;
            }

            if (!_dataQueue.TryDequeue(out var message))
            {
                await Task.Delay(10, cancellationToken);
                continue;
            }

            try
            {
                if (message.Contains(endStream))
                {
                    _logger.Log(LogLevel.Debug, "Ending stream and disconnecting");
                    _xmpp.State = new DisconnectState();
                    _xmpp.State.Execute(_xmpp);

                    if (message.Equals(endStream))
                    {
                        return;
                    }

                    message = message.Replace(endStream, string.Empty);
                }

                if (message.Contains("<stream:stream") && !message.Contains(endStream))
                {
                    _logger.Log(LogLevel.Debug, "Adding end tag");
                    message += endStream;
                }

                var context = new XmlParserContext(null, NamespaceManager, null, XmlSpace.None);
                using var reader = new XmlTextReader(message, XmlNodeType.Element, context);
                var root = XElement.Load(reader);
                var tag = _xmpp.TagRegistry.GetTag<Tag>(root);
                _logger.Log(LogLevel.Debug, $"Found tag {tag}");
                OnTag(tag);
            }
            catch (Exception ex)
            {
                _logger.Log(LogLevel.Error, $"Exception in ProcessQueueAsync: {ex.Message}");
            }
        }
    }

    private void ClientSocket_Data(object sender, DataEventArgs e)
    {
        _dataQueue.Enqueue(e.Message);
    }

    /// <summary>
    ///     Releases the unmanaged resources used by the <see cref="Parser"/> class and optionally releases the managed resources.
    /// </summary>
    /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
    private void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            _cts?.Cancel();
            _cts?.Dispose();
            _xmpp.ClientSocket.Data -= ClientSocket_Data;
        }

        _disposed = true;
    }
}
