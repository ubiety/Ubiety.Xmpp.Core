// Copyright 2019 Dieter Lunn
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
using Ubiety.Xmpp.Core.Sasl;
using Ubiety.Xmpp.Core.States;
using Ubiety.Xmpp.Core.Tags.Stream;

namespace Ubiety.Xmpp.Core.Infrastructure.Extensions
{
    /// <summary>
    /// Provides extension methods for the <see cref="Features"/> class to handle SSL, authentication, and resource binding functionalities during the XMPP connection process.
    /// </summary>
    public static class FeaturesExtensions
    {
        /// <summary>
        /// Checks whether SSL should be secured for the current XMPP connection.
        /// </summary>
        /// <param name="features">Current <see cref="Features"/> from the server.</param>
        /// <param name="xmpp">Current <see cref="XmppBase"/> instance.</param>
        /// <returns>A value indicating whether to secure the socket or not.</returns>
        public static bool CheckSsl(this Features features, XmppBase xmpp)
        {
            return features.StartTls != null &&
                   (xmpp.UseSsl || features.FeatureCount == 1 || features.StartTls.Required);
        }

        /// <summary>
        /// Authenticates the user using the supported SASL mechanisms provided in the server's features.
        /// </summary>
        /// <param name="features">The server's <see cref="Features"/> containing the supported SASL mechanisms.</param>
        /// <param name="client">The current <see cref="XmppClient"/> instance used for authentication.</param>
        public static void AuthenticateUser(this Features features, XmppClient client)
        {
            client.SaslProcessor = SaslProcessor.CreateProcessor(
                features.Mechanisms.SupportedTypes,
                MechanismTypes.Default,
                client);
            if (client.SaslProcessor is null)
            {
                client.State = new DisconnectState();
                client.State.Execute(client);
                return;
            }

            client.ClientSocket.Send(client.SaslProcessor.Initialize(client.Id, client.Password));
            client.State = new SaslState();
        }

        /// <summary>
        /// Initiates the resource binding process for the current XMPP connection.
        /// </summary>
        /// <param name="features">The current <see cref="Features"/> from the server, containing stream-related capabilities.</param>
        /// <param name="client">The current <see cref="XmppClient"/> instance used for the XMPP connection.</param>
        public static void StartBinding(this Features features, XmppClient client)
        {
            if (features.Bind.Required)
            {
            }
        }
    }
}
