using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core.States
{
    /// <summary>
    ///     Describes a state
    /// </summary>
    public interface IState
    {
        /// <summary>
        ///     Executes the current state
        /// </summary>
        void Execute(XmppBase xmpp, Tag tag = null);
    }
}
