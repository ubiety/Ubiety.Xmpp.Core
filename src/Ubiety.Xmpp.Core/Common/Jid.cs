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

using System;
using System.Text.RegularExpressions;
using Ubiety.Stringprep.Core;
using Ubiety.Xmpp.Core.Infrastructure.Exceptions;
using Ubiety.Xmpp.Core.Stringprep;

namespace Ubiety.Xmpp.Core.Common
{
    /// <summary>
    /// Represents a Jabber Identifier (JID) used in XMPP for identifying users, servers, and resources.
    /// </summary>
    /// <remarks>
    /// A JID consists of three main parts: username (optional), server, and resource (optional).
    /// The string representation of a JID follows the format: user@server/resource.
    /// </remarks>
    public sealed class Jid : IEquatable<Jid>
    {
        // language=regex
        private const string JidRegex = @"^(?:(?<username>.*)@)?(?<server>.*?)(?:\/(?<resource>.*))?$";
        private const string EscapeRegex = @"[@\\\/&:<>\s""']";
        private const string UnescapeRegex = @"\\([2-5][0267face])";

        private readonly IPreparationProcess _nameprep = NameprepProfile.Create();
        private readonly IPreparationProcess _nodeprep = NodeprepProfile.Create();
        private readonly IPreparationProcess _resourceprep = ResourceprepProfile.Create();

        private readonly string _resource = string.Empty;
        private readonly string _server;

        // Stores the username in the server escaped format
        private readonly string _user = string.Empty;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Jid" /> class.
        /// </summary>
        /// <param name="username">Username of the user.</param>
        /// <param name="server">XMPP server of the user.</param>
        /// <param name="resource">Server resource (if empty will be set by the server).</param>
        public Jid(string username, string server, string resource = "")
        {
            User = username ?? throw new ArgumentNullException(nameof(username));
            Server = server ?? throw new ArgumentNullException(nameof(server));
            Resource = resource;
        }

        private Jid()
        {
        }

        /// <summary>
        /// Gets the resource of the <see cref="Jid" />.
        /// </summary>
        /// <remarks>
        /// The resource is an optional part of the JID used to specify a particular session,
        /// device, or resource for the user. It is separated from the server part of the JID by a '/'.
        /// </remarks>
        public string Resource
        {
            get => _resource;
            private init => _resource = value is null ? null : _resourceprep.Run(value);
        }

        /// <summary>
        /// Gets the server domain of the <see cref="Jid" />.
        /// </summary>
        /// <remarks>
        /// The server represents the domain or host responsible for handling the XMPP communication
        /// for the user. It is an essential component of the JID and is subject to normalization
        /// processes during initialization.
        /// </remarks>
        public string Server
        {
            get => _server;
            private init => _server = value is null ? null : _nameprep.Run(value);
        }

        /// <summary>
        /// Gets the username portion of the <see cref="Jid" />.
        /// </summary>
        /// <remarks>
        /// The username, or user ID, is the part of the JID that identifies the specific user
        /// within their domain. It precedes the '@' symbol in the JID and is unescaped
        /// when accessed.
        /// </remarks>
        public string User
        {
            get => Unescape(_user);
            private init => _user = _nodeprep.Run(value);
        }

        /// <summary>
        /// Gets the full Jabber Identifier (JID) as a string representation.
        /// </summary>
        /// <remarks>
        /// The JID consists of the username, server, and optionally a resource in the format: user@server/resource.
        /// This property combines the different components of the JID into a single string.
        /// </remarks>
        public string Id => $"{_user}{(string.IsNullOrEmpty(_user) ? string.Empty : "@")}{_server}{(string.IsNullOrEmpty(_resource) ? string.Empty : "/")}{_resource}";

        /// <summary>
        /// Defines an implicit operator to convert a string to a <see cref="Jid" />.
        /// </summary>
        /// <param name="id">The string representation of a <see cref="Jid" /> to convert.</param>
        /// <returns>A new <see cref="Jid" /> instance created from the specified string.</returns>
        public static implicit operator Jid(string id)
        {
            return Parse(id, false);
        }

        /// <summary>
        /// Converts the specified <see cref="Jid" /> to its string representation.
        /// </summary>
        /// <param name="id">The <see cref="Jid" /> instance to convert.</param>
        /// <returns>The string representation of the specified <see cref="Jid" />.</returns>
        public static implicit operator string(Jid id)
        {
            return id.Id;
        }

        /// <summary>
        /// Determines whether two <see cref="Jid" /> instances are equal.
        /// </summary>
        /// <param name="one">First <see cref="Jid" /> to compare.</param>
        /// <param name="two">Second <see cref="Jid" /> to compare.</param>
        /// <returns>True if the <see cref="Jid" /> instances are equal; otherwise, false.</returns>
        public static bool operator ==(Jid one, Jid two)
        {
            return one != null && one.Equals(two);
        }

        /// <summary>
        /// Compares two <see cref="Jid"/> objects for inequality.
        /// </summary>
        /// <param name="one">First <see cref="Jid"/> to compare.</param>
        /// <param name="two">Second <see cref="Jid"/> to compare.</param>
        /// <returns>True if the JID objects are not equal; otherwise, false.</returns>
        public static bool operator !=(Jid one, Jid two)
        {
            return one != null && !one.Equals(two);
        }

        /// <summary>
        /// Parses a string JID into a <see cref="Jid" /> instance.
        /// </summary>
        /// <param name="value">The string representation of the JID to parse.</param>
        /// <param name="escaped">Indicates whether the provided JID is in an escaped format.</param>
        /// <returns>The parsed <see cref="Jid" /> instance.</returns>
        public static Jid Parse(string value, bool escaped)
        {
            if (!TryParse(value, escaped, out var jid))
            {
                throw new ParseException();
            }

            return jid;
        }

        /// <summary>
        /// Attempts to parse a string representation of a JID into a <see cref="Jid"/> object.
        /// </summary>
        /// <param name="value">The string representation of the JID to parse.</param>
        /// <param name="escaped">Indicates whether the JID string is escaped.</param>
        /// <param name="jid">The resulting <see cref="Jid"/> instance if parsing is successful; otherwise null.</param>
        /// <returns>True if the string representation is successfully parsed into a <see cref="Jid"/>; otherwise, false.</returns>
        public static bool TryParse(string value, bool escaped, out Jid jid)
        {
            var match = Regex.Match(value, JidRegex);

            if (!match.Success)
            {
                jid = null;
                return false;
            }

            jid = new Jid
            {
                User = escaped ? match.Groups["username"].Value : Escape(match.Groups["username"].Value),
                Server = match.Groups["server"].Value,
                Resource = match.Groups["resource"].Value,
            };

            return true;
        }

        /// <summary>
        /// Determines whether the current JID is equal to another JID.
        /// </summary>
        /// <param name="other">The JID to compare with the current JID.</param>
        /// <returns>true if the provided JID is equal to the current JID; otherwise, false.</returns>
        public bool Equals(Jid other)
        {
            return Id.Equals(other?.Id);
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current <see cref="Jid" /> instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current instance.</param>
        /// <returns>
        /// <c>true</c> if the specified object is equal to the current instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return obj switch
            {
                null => false,
                string _ => Id.Equals(obj),
                _ => obj is Jid jid && Id.Equals(jid.Id),
            };
        }

        /// <summary>
        /// Generates a hash code for the current instance of <see cref="Jid" />.
        /// </summary>
        /// <returns>
        /// An integer that represents the hash code of the current instance, based on the username, server, and resource.
        /// </returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(User, Server, Resource);
        }

        /// <summary>
        /// Converts the current <see cref="Jid"/> instance to its string representation in the format user@server/resource.
        /// </summary>
        /// <returns>The string representation of the JID.</returns>
        public override string ToString()
        {
            return Id;
        }

        private static string Escape(string username)
        {
            var re = new Regex(EscapeRegex);

            return re.Replace(username, Evaluator);

            static string Evaluator(Match m)
            {
                return m.Groups[0].Value switch
                {
                    " " => @"\20",
                    @"""" => @"\22",
                    "&" => @"\26",
                    "'" => @"\27",
                    "/" => @"\2f",
                    ":" => @"\3a",
                    "<" => @"\3c",
                    ">" => @"\3e",
                    "@" => @"\40",
                    "\\" => @"\5c",
                    _ => m.Groups[0].Value,
                };
            }
        }

        private static string Unescape(string username)
        {
            var re = new Regex(UnescapeRegex);

            return re.Replace(username, Evaluator);

            static string Evaluator(Match m)
            {
                return m.Groups[1].Value switch
                {
                    "20" => " ",
                    "22" => "\"",
                    "26" => "&",
                    "27" => "'",
                    "2f" => "/",
                    "3a" => ":",
                    "3c" => "<",
                    "3e" => ">",
                    "40" => "@",
                    "5c" => @"\",
                    _ => m.Groups[0].Value,
                };
            }
        }
    }
}
