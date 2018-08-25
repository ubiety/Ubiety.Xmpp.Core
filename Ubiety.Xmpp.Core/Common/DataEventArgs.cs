// <copyright file="DataEventArgs.cs" company="Dieter Lunn">
// Copyright (c) Dieter Lunn. All rights reserved.
// </copyright>

using System;

namespace Ubiety.Xmpp.Core.Common
{
    /// <inheritdoc />
    /// <summary>
    /// Socket data event arguments
    /// </summary>
    public class DataEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the message from the server
        /// </summary>
        public string Message { get; set; }
    }
}
