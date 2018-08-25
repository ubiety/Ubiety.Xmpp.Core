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

using Ubiety.Xmpp.Core.Logging;

namespace Ubiety.Xmpp.Core
{
    /// <summary>
    ///     Builds a new XmppClient
    /// </summary>
    public class XmppClientBuilder
    {
        private XmppClient _client;
        private ILogManager _logManager;

        /// <summary>
        ///     Begin the build process
        /// </summary>
        /// <returns>Builder instance</returns>
        public XmppClientBuilder Begin()
        {
            _client = new XmppClient();
            return this;
        }

        /// <summary>
        ///     Enable logging with the log manager
        /// </summary>
        /// <param name="manager">Log manager to use for logging</param>
        /// <returns>Builder instance</returns>
        public XmppClientBuilder EnableLogging(ILogManager manager)
        {
            _logManager = manager;
            return this;
        }

        /// <summary>
        ///     Builds the client
        /// </summary>
        /// <returns>Client with the options provided</returns>
        public XmppClient Build()
        {
            if (!(_logManager is null))
            {
                Log.Initialize(_logManager);
            }

            return _client;
        }
    }
}