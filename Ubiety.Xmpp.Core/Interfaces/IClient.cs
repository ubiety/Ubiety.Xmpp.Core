// <copyright file="IClient.cs" company="Dieter Lunn">
// Copyright (c) Dieter Lunn. All rights reserved.
// </copyright>

using Ubiety.Xmpp.Core.Common;

namespace Ubiety.Xmpp.Core.Interfaces
{
    /// <summary>
    ///     Defines a client interface
    /// </summary>
    public interface IClient
    {
        /// <summary>
        ///     Gets or sets the user JID
        /// </summary>
        Jid Id { get; set; }

        /// <summary>
        /// Gets or sets the port of the server
        /// </summary>
        int Port { get; set; }

        /// <summary>
        /// Gets value indicating whether the socket should use SSL
        /// </summary>
        bool UseSsl { get; }
    }
}