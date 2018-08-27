using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Logging;
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core.States
{
    /// <summary>
    ///     Connecting state for the protocol
    /// </summary>
    /// <inheritdoc />
    public class ConnectingState : IState
    {
        private static readonly ILog Logger;

        static ConnectingState()
        {
            Logger = Log.Get<ConnectingState>();
        }

        /// <inheritdoc />
        public void Execute(XmppBase xmpp, Tag tag = null)
        {
            if (xmpp is XmppClient client)
            {
                Logger.Log(LogLevel.Debug, "Connecting to server");
                client.ClientSocket.Connect(client.Id);                
            }
        }
    }
}
