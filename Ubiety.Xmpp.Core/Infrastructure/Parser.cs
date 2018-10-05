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
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.States;
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core.Infrastructure
{
    /// <summary>
    ///     XMPP protocol parser
    /// </summary>
    public sealed class Parser
    {
        private readonly Queue<string> _dataQueue;
        private readonly ILog _logger = Log.Get<Parser>();
        private readonly XmppBase _xmpp;
        private XmlNamespaceManager _namespaceManager;
        private bool _running;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Parser" /> class
        /// </summary>
        /// <param name="xmpp">XMPP instance</param>
        public Parser(XmppBase xmpp)
        {
            _xmpp = xmpp;
            _dataQueue = new Queue<string>();
            _xmpp.ClientSocket.Data += ClientSocket_Data;
            _logger.Log(LogLevel.Debug, "Parser created");
        }

        /// <summary>
        ///     Tag event
        /// </summary>
        public event EventHandler<TagEventArgs> Tag;

        private XmlNamespaceManager NamespaceManager
        {
            get
            {
                if (_namespaceManager is null)
                {
                    _namespaceManager = new XmlNamespaceManager(new NameTable());
                    _namespaceManager.AddNamespace(string.Empty, Namespaces.Client);
                    _namespaceManager.AddNamespace("stream", Namespaces.Stream);
                }

                return _namespaceManager;
            }
        }

        /// <summary>
        ///     Starts the parsing process
        /// </summary>
        public void Start()
        {
            _logger.Log(LogLevel.Debug, "Starting parsing process");
            _running = true;
            Task.Run(() => ProcessQueue());
        }

        /// <summary>
        ///     Stop the parsing process
        /// </summary>
        public void Stop()
        {
            _running = false;
        }

        private void OnTag(Tag tag)
        {
            Tag?.Invoke(this, new TagEventArgs { Tag = tag });
        }

        private void ProcessQueue()
        {
            const string endStream = "</stream:stream>";

            while (true)
            {
                if (_xmpp.State is DisconnectedState || !_running)
                {
                    _logger.Log(LogLevel.Debug, "Disconnected or stopped");
                    break;
                }

                if (_dataQueue.Count <= 0)
                {
                    continue;
                }

                var message = _dataQueue.Dequeue();

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
                var reader = new XmlTextReader(message, XmlNodeType.Element, context);

                var root = XElement.Load(reader);

                var tag = _xmpp.Registry.GetTag<Tag>(root);
                _logger.Log(LogLevel.Debug, $"Found tag {tag}");

                OnTag(tag);
            }
        }

        private void ClientSocket_Data(object sender, DataEventArgs e)
        {
            _dataQueue.Enqueue(e.Message);
        }
    }
}