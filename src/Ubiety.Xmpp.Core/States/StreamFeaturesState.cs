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
    ///     Stream features state.
    /// </summary>
    public class StreamFeaturesState : IState
    {
        private static readonly ILog Logger = Log.Get<StreamFeaturesState>();

        /// <inheritdoc />
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

            if (xmpp is XmppClient client && !client.Authenticated)
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
