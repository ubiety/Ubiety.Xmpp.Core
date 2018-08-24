// <copyright file="Address.cs" company="Dieter Lunn">
// Copyright (c) Dieter Lunn. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Records;
using Ubiety.Dns.Core.Records.General;
using Ubiety.Xmpp.Core.Interfaces;

namespace Ubiety.Xmpp.Core.Net
{
    /// <summary>
    ///     Address class
    /// </summary>
    internal class Address
    {
        private readonly IClient _client;
        private readonly Resolver _resolver;
        private int _srvAttempts;
        private bool _srvFailed;
        private List<RecordSrv> _srvRecords;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Address" /> class
        /// </summary>
        /// <param name="client">Instance of the main client</param>
        public Address(IClient client)
        {
            _resolver = new Resolver("8.8.8.8") { UseCache = true, Timeout = 5, TransportType = TransportType.Tcp };
            _client = client;
        }

        /// <summary>
        ///     Gets a value indicating whether the address is IPv6
        /// </summary>
        public bool IsIPv6 { get; private set; }

        /// <summary>
        ///     Gets the hostname of the address
        /// </summary>
        public string Hostname { get; private set; }

        /// <summary>
        ///     Gets the next IP address for the server
        /// </summary>
        /// <returns><see cref="IPAddress" /> of the XMPP server</returns>
        public IPAddress NextIpAddress()
        {
            Hostname = !string.IsNullOrEmpty(_client.Id.Server) ? _client.Id.Server : string.Empty;

            if (IPAddress.TryParse(Hostname, out var address))
            {
                return address;
            }

            if (_srvRecords is null && !_srvFailed)
            {
                _srvRecords = ResolveSrv();
            }

            if (_srvFailed || _srvRecords is null)
            {
                return Resolve();
            }

            if (_srvAttempts >= _srvRecords.Count)
            {
                return null;
            }

            var ip = Resolve(_srvRecords[_srvAttempts].Target);
            if (ip is null)
            {
                _srvAttempts++;
            }
            else
            {
                return ip;
            }

            return null;
        }

        private IPAddress Resolve(string hostname = "")
        {
            Response response = null;
            var host = string.IsNullOrEmpty(hostname) ? Hostname : hostname;

            if (Socket.OSSupportsIPv6)
            {
                response = _resolver.Query(host, QuestionType.AAAA, QuestionClass.IN);
            }

            if (response?.Answers.Count > 0)
            {
                IsIPv6 = true;
                return ((RecordAaaa)response.Answers[0].Record).Address;
            }

            response = _resolver.Query(host, QuestionType.A, QuestionClass.IN);
            return response.Answers.Select(answer => answer.Record).OfType<RecordA>().Select(a => a.Address)
                .FirstOrDefault();
        }

        private List<RecordSrv> ResolveSrv()
        {
            var response = _resolver.Query($"_xmpp-client._tcp.{Hostname}", QuestionType.SRV, QuestionClass.IN);

            if (response.Header.AnswerCount > 0)
            {
                _srvFailed = false;
                return response.Answers.Select(record => record.Record as RecordSrv).ToList();
            }

            _srvFailed = true;
            return null;
        }
    }
}