// <copyright file="XmppClient.cs" company="Dieter Lunn">
// Copyright (c) Dieter Lunn. All rights reserved.
// </copyright>

using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Interfaces;

namespace Ubiety.Xmpp.Core
{
    /// <summary>
    ///     Main XMPP client class
    /// </summary>
    public class XmppClient : IClient
    {
        /// <inheritdoc cref="Id" />
        public Jid Id { get; set; }
    }
}