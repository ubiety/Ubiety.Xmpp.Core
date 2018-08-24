// <copyright file="Address.cs" company="Dieter Lunn">
// Copyright (c) Dieter Lunn. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Net;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Records;

namespace Ubiety.Xmpp.Core.Net
{
    internal class Address
    {
        private readonly Resolver resolver;
        private int srvAttempts;
        private bool srvFailed;
        private List<RecordSrv> srvRecords;

        public Address()
        {
            this.resolver = new Resolver("8.8.8.8") { UseCache = true, Timeout = 5, TransportType = TransportType.Tcp };
        }

        public bool IsIPv6 { get; private set; }

        public string Hostname { get; private set; }

        public IPAddress NextIpAddress() {}
    }
}
