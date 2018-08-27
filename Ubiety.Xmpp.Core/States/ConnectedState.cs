using System.Xml.Linq;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core.States
{
    /// <summary>
    ///     Connected to the server state
    /// </summary>
    /// <inheritdoc />
    public class ConnectedState : IState
    {
        /// <inheritdoc />
        public void Execute(XmppBase xmpp, Tag tag = null)
        {
            var stream = xmpp.Registry.GetTag<Stream>(XName.Get("stream", Namespaces.Stream));

            xmpp.ClientSocket.Send(stream.StartTag);
        }
    }
}
