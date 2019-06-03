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
    ///     Extension methods for the features tag.
    /// </summary>
    public static class FeaturesExtensions
    {
        /// <summary>
        ///     Check if SSL is required or requested.
        /// </summary>
        /// <param name="features">Current <see cref="Features"/> from the server.</param>
        /// <param name="xmpp">Current <see cref="XmppBase"/> instance.</param>
        /// <returns>A value indicating whether to secure the socket or not.</returns>
        public static bool CheckSsl(this Features features, XmppBase xmpp)
        {
            if (features.StartTls != null && (xmpp.UseSsl || features.FeatureCount == 1 || features.StartTls.Required))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Authenticate a user with the server.
        /// </summary>
        /// <param name="features">Current <see cref="Features"/> from the server.</param>
        /// <param name="client">Current <see cref="XmppClient"/> instance.</param>
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
        ///     Start user resource binding.
        /// </summary>
        /// <param name="features">Current <see cref="Features"/> from the server.</param>
        /// <param name="client">Current <see cref="XmppClient"/> instance.</param>
        public static void StartBinding(this Features features, XmppClient client)
        {
            if (features.Bind.Required)
            {
            }
        }
    }
}
