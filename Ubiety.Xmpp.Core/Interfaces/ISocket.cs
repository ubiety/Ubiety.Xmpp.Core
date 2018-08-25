namespace Ubiety.Xmpp.Core.Interfaces
{
    /// <summary>
    /// Defines a socket interface
    /// </summary>
    public interface ISocket
    {
        /// <summary>
        /// Connect to an XMPP server
        /// </summary>
        /// <param name="client">Client to use for the connection</param>
        void Connect(IClient client);

        /// <summary>
        /// Disconnects from the server
        /// </summary>
        void Disconnect();
    }
}
