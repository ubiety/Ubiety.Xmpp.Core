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

namespace Ubiety.Xmpp.Core
{
    /// <summary>
    ///     Builds an XMPP connection
    /// </summary>
    public class XmppBuilder
    {
        /// <summary>
        ///     Begins the build process
        /// </summary>
        /// <returns>Returns the current instance</returns>
        public XmppBuilder Begin()
        {
            return this;
        }

        /// <summary>
        ///     Build a client connection
        /// </summary>
        /// <returns>Returns a new <see cref="XmppClientBuilder" /> instance</returns>
        public XmppClientBuilder BuildClient()
        {
            return new XmppClientBuilder();
        }

        /// <summary>
        ///     Build a server connection
        /// </summary>
        public void BuildServer()
        {
            throw new NotImplementedException();
        }
    }
}