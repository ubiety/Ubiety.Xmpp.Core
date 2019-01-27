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
using Ubiety.Xmpp.Core.Infrastructure.Extensions;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Sasl;
using Ubiety.Xmpp.Core.Tags;
using Ubiety.Xmpp.Core.Tags.Stream;

namespace Ubiety.Xmpp.Core.States
{
    /// <inheritdoc />
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
                    return;
            }

            if (!xmpp.ClientSocket.Secure)
            {
                Logger.Log(LogLevel.Debug, "Checking if we should use SSL");
                if (features.CheckSsl(xmpp))
                {
                    return;
                }
            }

            if (xmpp is XmppClient client && !client.Authenticated)
            {
                Logger.Log(LogLevel.Debug, "Authenticating the user");
                features.AuthenticateUser(client);
            }
        }
    }
}
