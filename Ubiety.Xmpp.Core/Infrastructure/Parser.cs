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
using Ubiety.Xmpp.Core.Infrastructure.Extensions;
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
        private readonly ILog _logger;
        private readonly XmppBase _xmpp;
        private XmlNamespaceManager _namespaceManager;
        private bool _running;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Parser" /> class
        /// </summary>
        public Parser(XmppBase xmpp)
        {
            _logger = Log.Get<Parser>();
            _xmpp = xmpp;
            _dataQueue = new Queue<string>();
            _xmpp.ClientSocket.Data += ClientSocket_Data;
            _logger.Log(LogLevel.Debug, "Parser created");
        }

        private XmlNamespaceManager NamespaceManager
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

        private void OnTag(Tag tag)
        {
            Tag?.Invoke(this, new TagEventArgs {Tag = tag});
        }

        private void ProcessQueue()
        {
            while (true)
            {
                if (_xmpp.State is DisconnectedState || !_running) break;

                if (_dataQueue.Count <= 0) continue;
                var message = _dataQueue.Dequeue();

                if (message.Contains("<stream:stream") && !message.Contains("</stream:stream>"))
                {
                    _logger.Log(LogLevel.Debug, "Adding end tag");
                    message += "</stream:stream>";
                }

                if (message.Equals("</stream:stream>"))
                {
                    _logger.Log(LogLevel.Debug, "Ending stream and disconnecting");
                    _xmpp.State = new DisconnectState();
                    _xmpp.State.Execute(_xmpp);
                    return;
                }

                var element = ParseMessage(message);

                OnTag(ParseTag(element));
            }
        }

        private string ParseMessage(string message)
        {
            do
            {
                var tagOpenPosition = message.FirstUnescaped('<');
                var tagClosePosition = message.FirstUnescaped('>');
                var firstSpace = message.FirstUnescaped(' ');

                var valid = false;
                var elementEndPosition = 0;

                if (tagOpenPosition == -1 || tagClosePosition == -1) return string.Empty;

                if (message.Substring(tagOpenPosition, 2) == "<?" &&
                    message.Substring(tagClosePosition - 1, 2) == "?>" || message[tagOpenPosition + 1] == '/')
                {
                    // Empty element
                    elementEndPosition += tagClosePosition + 1;
                }
                else if (message[tagClosePosition - 1] == '/')
                {
                    // Self closing tag
                    elementEndPosition += tagClosePosition + 1;
                    valid = true;
                }
                else
                {
                    // Open tag
                    var nameLength = -1;

                    if (tagClosePosition == -1 || firstSpace < tagClosePosition)
                        nameLength = firstSpace;
                    else if (firstSpace == -1 || tagClosePosition < firstSpace) nameLength = tagClosePosition;

                    if (nameLength == -1) return string.Empty;

                    var elementName = message.Substring(tagOpenPosition + 1, nameLength - 1);
                    var endTagPosition = FindEndTag(message.Substring(tagClosePosition + 1), elementName);

                    var absoluteEnd = endTagPosition + tagClosePosition + 1;

                    if (endTagPosition != -1 && absoluteEnd <= message.Length)
                    {
                        elementEndPosition = absoluteEnd;
                        valid = true;
                    }
                    else
                    {
                        return string.Empty;
                    }
                }

                var element = message.Substring(0, elementEndPosition);

                return valid ? element : string.Empty;

            } while (!String.IsNullOrEmpty(message));
        }

        private int FindEndTag(string data, string elementName)
        {
            throw new NotImplementedException();
        }

        private Tag ParseTag(string message)
        {
            try
            {
                var document = new XDocument();
                var reader = new StringReader(message);
                var context = new XmlParserContext(null, NamespaceManager, null, XmlSpace.None);
                var xmlReader = XmlReader.Create(reader,
                    new XmlReaderSettings {ConformanceLevel = ConformanceLevel.Fragment, IgnoreWhitespace = true},
                    context);

                xmlReader.MoveToContent();
                while (xmlReader.ReadState != ReadState.EndOfFile) document.Add(XNode.ReadFrom(xmlReader));

                return _xmpp.Registry.GetTag<Tag>(document.Root?.Name);
            }
            catch (Exception e)
            {
                _logger.Log(LogLevel.Error, e, "Error parsing tag");
                throw;
            }
        }

        private void ClientSocket_Data(object sender, DataEventArgs e)
        {
            _dataQueue.Enqueue(e.Message);
        }
    }
}