// <copyright file="ISocket.cs" company="Dieter Lunn">
// Copyright (c) Dieter Lunn. All rights reserved.
// </copyright>

using System;
using Ubiety.Xmpp.Core.Common;

namespace Ubiety.Xmpp.Core.Interfaces
{
    /// <summary>
    /// Defines a socket interface
    /// </summary>
    public interface ISocket
    {
        /// <summary>
        /// Raised when data is received from the server
        /// </summary>
        event EventHandler<DataEventArgs> Data;

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
