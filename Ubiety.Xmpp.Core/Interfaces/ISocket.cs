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
        /// Raised when the socket is connected to the server
        /// </summary>
        event EventHandler Connection;

        /// <summary>
        /// Gets a value indicating whether the socket is connected
        /// </summary>
        bool Connected { get; }

        /// <summary>
        /// Connect to an XMPP server
        /// </summary>
        void Connect();

        /// <summary>
        /// Disconnects from the server
        /// </summary>
        void Disconnect();
    }
}
