// <copyright file="XmppClientBuilder.cs" company="Dieter Lunn">
// Copyright (c) Dieter Lunn. All rights reserved.
// </copyright>

namespace Ubiety.Xmpp.Core
{
    /// <summary>
    ///     Builds a new XmppClient
    /// </summary>
    public class XmppClientBuilder
    {
        private XmppClient _client;

        /// <summary>
        ///     Begin the build process
        /// </summary>
        /// <returns>Builder instance</returns>
        public XmppClientBuilder Begin()
        {
            _client = new XmppClient();
            return this;
        }

        /// <summary>
        ///     Builds the client
        /// </summary>
        /// <returns>Client with the options provided</returns>
        public XmppClient Build()
        {
            return _client;
        }
    }
}