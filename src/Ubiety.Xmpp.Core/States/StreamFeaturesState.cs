// Copyright 2018, 2019 Dieter Lunn
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
using Ubiety.Xmpp.Core.Infrastructure.Exceptions;
using Ubiety.Xmpp.Core.Infrastructure.Extensions;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Tags;
using Ubiety.Xmpp.Core.Tags.Stream;

namespace Ubiety.Xmpp.Core.States
{
    /// <summary>
    /// Represents the state responsible for handling stream features in the XMPP protocol.
    /// </summary>
    /// <remarks>
    /// This state processes the server's stream features and determines the next steps in the XMPP state flow.
    /// It involves checking for and handling SSL/TLS, user authentication, and resource binding.
    /// </remarks>
    public class StreamFeaturesState : IState
    {
        private static readonly ILog Logger = Log.Get<StreamFeaturesState>();

        /// <summary>
        /// Executes the current state logic using the given XMPP base instance and optional tag.
        /// Processes the features tag and handles SSL security, user authentication, and transition to the next state.
        /// </summary>
        /// <param name="xmpp">The XMPP base instance managing the connection and state transitions.</param>
        /// <param name="tag">Optional tag to be processed; used for determining further state-specific actions.</param>
        /// <exception cref="InvalidStateException">Thrown if the tag provided is not valid for the current state.</exception>
        public void Execute(XmppBase xmpp, Tag tag = null)
        {
            Features features;

            xmpp.ClientSocket.SetReadClear();

            Logger.Log(LogLevel.Debug, "Starting to parse features");
            switch (tag)
            {
                case Stream s when s.Version.StartsWith("1."):
                    features = s.Features;
                    break;

                case Features f:
                    features = f;
                    break;

                default:
                    Logger.Log(LogLevel.Error, "Unexpected tag. Wrong state executed");
                    throw new InvalidStateException("Received tag that is not valid for the current state");
            }

            if (!xmpp.ClientSocket.Secure)
            {
                Logger.Log(LogLevel.Debug, "Socket is not secure. Checking if we should use SSL");
                if (features.CheckSsl(xmpp))
                {
                    Logger.Log(LogLevel.Debug, "Initializing security...");
                    xmpp.State = new StartTlsState();
                    xmpp.State.Execute(xmpp);
                    return;
                }
            }

            if (xmpp is XmppClient { Authenticated: false } client)
            {
                Logger.Log(LogLevel.Debug, "Authenticating the user");
                features.AuthenticateUser(client);
            }

            Logger.Log(LogLevel.Debug, "Starting resource binding");
            xmpp.State = new BindingState();
            xmpp.State.Execute(xmpp);
        }
    }
}
