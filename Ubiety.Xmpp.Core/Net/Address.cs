// Copyright 2018 Dieter Lunn
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Ubiety.Dns.Core;
using Ubiety.Dns.Core.Common;
using Ubiety.Dns.Core.Records;
using Ubiety.Dns.Core.Records.General;
using Ubiety.Xmpp.Core.Logging;

namespace Ubiety.Xmpp.Core.Net
{
    /// <summary>
    ///     Address class
    /// </summary>
    internal class Address
    {
        private readonly IClient _client;
        private readonly ILog _logger = Log.Get<Address>();
        private readonly Resolver _resolver;
        private int _srvAttempts;
        private bool _srvFailed;
        private List<RecordSrv> _srvRecords;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Address" /> class
        /// </summary>
        /// <param name="client"><see cref="IClient" /> for configuration</param>
        public Address(IClient client)
        {
            _logger.Log(LogLevel.Debug, $"{typeof(Address)} created");
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
            _logger.Log(LogLevel.Debug, "NextIpAddress() called");
            Hostname = !string.IsNullOrEmpty(_client.Id.Server) ? _client.Id.Server : string.Empty;

            _logger.Log(LogLevel.Debug, $"Resolving address for domain: {Hostname}");

            if (IPAddress.TryParse(Hostname, out var address))
            {
                _logger.Log(LogLevel.Debug, "Hostname is an IP address");
                return address;
            }

            if (_srvRecords is null && !_srvFailed)
            {
                _logger.Log(LogLevel.Debug, "Searching for SRV records");
                _srvRecords = ResolveSrv();
            }

            if (_srvFailed || _srvRecords is null)
            {
                _logger.Log(LogLevel.Debug, "No SRV records, trying standard DNS resolution");
                return Resolve();
            }

            if (_srvAttempts >= _srvRecords.Count)
            {
                return null;
            }

            _logger.Log(LogLevel.Debug, "Resolving the next SRV record");
            var ip = Resolve(_srvRecords[_srvAttempts].Target);
            if (ip is null)
            {
                _srvAttempts++;
            }
            else
            {
                _logger.Log(LogLevel.Debug, $"Found IP: {ip}");
                return ip;
            }

            return null;
        }

        private IPAddress Resolve(string hostname = "")
        {
            _logger.Log(LogLevel.Debug, "Resolve(string) called");
            Response response = null;
            var host = string.IsNullOrEmpty(hostname) ? Hostname : hostname;

            if (Socket.OSSupportsIPv6 && _client.UseIPv6)
            {
                _logger.Log(LogLevel.Debug, "Resolving an AAAA address as IPv6 is supported and enabled");
                response = _resolver.Query(host, QuestionType.AAAA, QuestionClass.IN);
            }

            if (response?.Answers.Count > 0)
            {
                _logger.Log(LogLevel.Debug, $"IPv6 address found for {Hostname}");
                IsIPv6 = true;
                return ((RecordAaaa)response.Answers[0].Record).Address;
            }

            _logger.Log(LogLevel.Debug, "Resolving a standard IPv4 A record");
            response = _resolver.Query(host, QuestionType.A, QuestionClass.IN);
            _logger.Log(LogLevel.Debug, "IP found");
            return response.Answers.Select(answer => answer.Record).OfType<RecordA>().Select(a => a.Address)
                .FirstOrDefault();
        }

        private List<RecordSrv> ResolveSrv()
        {
            _logger.Log(LogLevel.Debug, "ResolveSrv() called");
            _logger.Log(LogLevel.Debug, $"Attempting to retrieve any XMPP SRV records for {Hostname}");
            var response = _resolver.Query($"_xmpp-client._tcp.{Hostname}", QuestionType.SRV, QuestionClass.IN);

            if (response.Header.AnswerCount > 0)
            {
                _logger.Log(LogLevel.Debug, "SRV records found");
                _srvFailed = false;
                return response.Answers.Select(record => record.Record as RecordSrv).ToList();
            }

            _logger.Log(LogLevel.Debug, $"No SRV records found for {Hostname}");
            _srvFailed = true;
            return new List<RecordSrv>();
        }
    }
}