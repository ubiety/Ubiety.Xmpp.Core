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

using Ubiety.Xmpp.Core.Common;

namespace Ubiety.Xmpp.Core
{
    /// <summary>
    ///     Defines a client interface
    /// </summary>
    public interface IClient
    {
        /// <summary>
        ///     Gets or sets the user JID
        /// </summary>
        Jid Id { get; set; }

        /// <summary>
        ///     Gets or sets the port of the server
        /// </summary>
        int Port { get; set; }

        /// <summary>
        ///     Gets a value indicating whether the socket should use SSL
        /// </summary>
        bool UseSsl { get; }
    }
}