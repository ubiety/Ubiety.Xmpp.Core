// Copyright 2025 Dieter Lunn
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

namespace Ubiety.Xmpp.Core.Server
{
    /// <summary>
    ///     Represents a server in the XMPP core infrastructure.
    /// </summary>
    internal interface IServer
    {
        /// <summary>
        ///     Gets the port number on which the server is listening.
        /// </summary>
        int Port { get; }

        /// <summary>
        ///     Starts the server and begins listening for incoming connections.
        /// </summary>
        void Start();

        /// <summary>
        ///     Stops the server and ceases listening for incoming connections.
        /// </summary>
        void Stop();
    }
}
