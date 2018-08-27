using System;

namespace Ubiety.Xmpp.Core
{
    /// <summary>
    ///     Builds an XMPP connection
    /// </summary>
    public class XmppBuilder
    {
        /// <summary>
        ///     Begins the build process
        /// </summary>
        /// <returns></returns>
        public XmppBuilder Begin()
        {
            return this;
        }

        /// <summary>
        ///     Build a client connection
        /// </summary>
        /// <returns></returns>
        public XmppClientBuilder BuildClient()
        {
            return new XmppClientBuilder();
        }

        /// <summary>
        ///     Build a server connection
        /// </summary>
        public void BuildServer()
        {
            throw new NotImplementedException();
        }
    }
}
