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
using System.IO;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Ubiety.Xmpp.Core.Common;
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
        private readonly XmppBase _xmpp;
        private XmlParserContext _context;
        private XmlNamespaceManager _namespaceManager;
        private bool _running;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Parser" /> class
        /// </summary>
        public Parser(XmppBase xmpp)
        {
            _xmpp = xmpp;
            _dataQueue = new Queue<string>();
            _xmpp.ClientSocket.Data += ClientSocket_Data;
        }

        /// <summary>
        ///     Gets the current XML context
        /// </summary>
        public XmlParserContext Context =>
            _context ?? (_context = new XmlParserContext(null, NamespaceManager, null, XmlSpace.None));

        /// <summary>
        ///     Gets the current XML namespace manager
        /// </summary>
        public XmlNamespaceManager NamespaceManager
        {
            get
            {
                if (!(_namespaceManager is null)) return _namespaceManager;
                _namespaceManager = new XmlNamespaceManager(new NameTable());
                _namespaceManager.AddNamespace("", Namespaces.Client);
                _namespaceManager.AddNamespace("stream", Namespaces.Stream);

                return _namespaceManager;
            }
        }

        /// <summary>
        ///     Tag event
        /// </summary>
        public event EventHandler<TagEventArgs> Tag;

        /// <summary>
        ///     Starts the parsing process
        /// </summary>
        public void Start()
        {
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

        /// <summary>
        ///     Raise the tag event
        /// </summary>
        /// <param name="tag">Tag to send</param>
        public void OnTag(Tag tag)
        {
            Tag?.Invoke(this, new TagEventArgs {Tag = tag});
        }

        private void ProcessQueue()
        {
            while (true)
            {
                if (_xmpp.State is DisconnectedState || !_running) break;

                var message = _dataQueue.Dequeue();

                if (message.Equals("</stream:stream>")) _xmpp.State = new DisconnectedState();

                var tag = ParseTag(message);

                OnTag(tag);
            }
        }

        private Tag ParseTag(string message)
        {
            try
            {
                var document = new XDocument();
                var reader = new StringReader(message);
                var xmlReader = XmlReader.Create(reader,
                    new XmlReaderSettings {ConformanceLevel = ConformanceLevel.Fragment, IgnoreWhitespace = true},
                    _context);

                xmlReader.MoveToContent();
                while (xmlReader.ReadState != ReadState.EndOfFile) document.Add(XNode.ReadFrom(xmlReader));

                return _xmpp.Registry.GetTag<Tag>(document.Root?.Name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private void ClientSocket_Data(object sender, DataEventArgs e)
        {
            _dataQueue.Enqueue(e.Message);
        }
    }
}