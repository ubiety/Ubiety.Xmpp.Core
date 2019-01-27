using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Sasl;
using Ubiety.Xmpp.Core.States;
using Ubiety.Xmpp.Core.Tags.Stream;

namespace Ubiety.Xmpp.Core.Infrastructure.Extensions
{
    /// <summary>
    ///     Extension methods for the features tag
    /// </summary>
    public static class FeaturesExtensions
    {
        /// <summary>
        ///     Check if SSL is required or requested
        /// </summary>
        /// <param name="features">Features tag to use when checking</param>
        /// <param name="xmpp">Xmpp class to execute state</param>
        /// <returns>Indicates whether we secured the socket or not</returns>
        public static bool CheckSsl(this Features features, XmppBase xmpp)
        {
            if (features.StartTls != null && (xmpp.UseSsl || features.FeatureCount == 1 || features.StartTls.Required))
            {
                xmpp.State = new StartTlsState();
                xmpp.State.Execute(xmpp);
                return true;
            }

            return false;
        }

        /// <summary>
        ///     Authenticate a user with the server
        /// </summary>
        /// <param name="features">Features to use for authentication</param>
        /// <param name="client">Client to use</param>
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
    }
}
