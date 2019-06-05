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
using System.Text;
using System.Text.RegularExpressions;
using Ubiety.Stringprep.Core;
using Ubiety.Xmpp.Core.Infrastructure.Exceptions;
using Ubiety.Xmpp.Core.Stringprep;

namespace Ubiety.Xmpp.Core.Common
{
    /// <inheritdoc />
    /// <summary>
    ///     Jabber ID.
    /// </summary>
    public sealed class Jid : IEquatable<Jid>
    {
        // language=regex
        private const string JidRegex = @"^(?:(?<username>.*)@)?(?<server>.*?)(?:\/(?<resource>.*))?$";

        private readonly IPreparationProcess _nameprep = NameprepProfile.Create();
        private readonly IPreparationProcess _nodeprep = NodeprepProfile.Create();
        private readonly IPreparationProcess _resourceprep = ResourceprepProfile.Create();

        private string _id;
        private string _resource;
        private string _server;
        private string _user;

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
        ///     Gets the JID resource.
        /// </summary>
        public string Resource
        {
            get => _resource;
            private set => _resource = value is null ? null : _resourceprep.Run(value);
        }

        /// <summary>
        ///     Gets the server of the JID.
        /// </summary>
        public string Server
        {
            get => _server;
            private set => _server = value is null ? null : _nameprep.Run(value);
        }

        /// <summary>
        ///     Gets the user name of the JID.
        /// </summary>
        public string User
        {
            get => Unescape(_user);
            private set
            {
                var temp = Escape(value);
                _user = _nodeprep.Run(temp);
            }
        }

        /// <summary>
        ///     Gets the <see cref="Jid" /> as a string.
        /// </summary>
        public string Id
        {
            get => _id ?? BuildJid();
        }

        /// <summary>
        ///     Implicitly converts a string into a <see cref="Jid" />.
        /// </summary>
        /// <param name="id">String version of the <see cref="Jid" />.</param>
        public static implicit operator Jid(string id)
        {
            return Parse(id);
        }

        /// <summary>
        ///     Implicitly converts a <see cref="Jid" /> to a string.
        /// </summary>
        /// <param name="id"><see cref="Jid" /> of the ID.</param>
        public static implicit operator string(Jid id)
        {
            return id.Id;
        }

        /// <summary>
        ///     Compares equality of one <see cref="Jid" /> to another.
        /// </summary>
        /// <param name="one">First <see cref="Jid" />.</param>
        /// <param name="two">Second <see cref="Jid" />.</param>
        /// <returns>True if the Jids are equal.</returns>
        public static bool operator ==(Jid one, Jid two)
        {
            return one != null && one.Equals(two);
        }

        /// <summary>
        ///     Compares the inequality of one <see cref="Jid" /> to another.
        /// </summary>
        /// <param name="one">First <see cref="Jid" />.</param>
        /// <param name="two">Second <see cref="Jid" />.</param>
        /// <returns>True if the Jids are not equal.</returns>
        public static bool operator !=(Jid one, Jid two)
        {
            return one != null && !one.Equals(two);
        }

        /// <summary>
        ///     Parse a string into a <see cref="Jid" />.
        /// </summary>
        /// <param name="value">String jid to parse.</param>
        /// <returns><see cref="Jid" /> of the string.</returns>
        public static Jid Parse(string value)
        {
            if (!TryParse(value, out var jid))
            {
                throw new ParseException();
            }

            return jid;
        }

        /// <summary>
        ///     Try to parse the string into a <see cref="Jid" />.
        /// </summary>
        /// <param name="value">String jid to parse.</param>
        /// <param name="jid">Parsed <see cref="Jid" />.</param>
        /// <returns>True if parse is successful; otherwise false.</returns>
        public static bool TryParse(string value, out Jid jid)
        {
            var match = Regex.Match(value, JidRegex);

            if (!match.Success)
            {
                jid = default;
                return false;
            }

            jid = new Jid
            {
                User = match.Groups["username"].Value,
                Server = match.Groups["server"].Value,
                Resource = match.Groups["resource"].Value,
            };

            return true;
        }

        /// <inheritdoc />
        public bool Equals(Jid other)
        {
            return Id.Equals(other.Id);
        }

        /// <inheritdoc />
        public override bool Equals(object obj)
        {
            switch (obj)
            {
                case null:
                    return false;

                case string _:
                    return Id.Equals(obj);
            }

            return obj is Jid jid && Id.Equals(jid.Id);
        }

        /// <inheritdoc />
        public override int GetHashCode()
        {
            unchecked
            {
                var hash = 17;

                hash = (hash * 23) + User.GetHashCode();
                hash = (hash * 23) + Server.GetHashCode();
                hash = (hash * 23) + Resource.GetHashCode();

                return hash;
            }
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return Id;
        }

        private static string Escape(string user)
        {
            var re = new Regex(@"[@\\\/&:<>\s""']");

            string Evaluator(Match m)
            {
                switch (m.Groups[0].Value)
                {
                    case " ":
                        return @"\20";
                    case @"""":
                        return @"\22";
                    case "&":
                        return @"\26";
                    case "'":
                        return @"\27";
                    case "/":
                        return @"\2f";
                    case ":":
                        return @"\3a";
                    case "<":
                        return @"\3c";
                    case ">":
                        return @"\3e";
                    case "@":
                        return @"\40";
                    case "\\":
                        return @"\5c";
                    default:
                        return m.Groups[0].Value;
                }
            }

            return re.Replace(user, Evaluator);
        }

        private string Unescape(string username)
        {
            var re = new Regex(@"\\([2-5][0267face])");

            string Evaluator(Match m)
            {
                switch (m.Groups[1].Value)
                {
                    case "20":
                        return " ";
                    case "22":
                        return "\"";
                    case "26":
                        return "&";
                    case "27":
                        return "'";
                    case "2f":
                        return "/";
                    case "3a":
                        return ":";
                    case "3c":
                        return "<";
                    case "3e":
                        return ">";
                    case "40":
                        return "@";
                    case "5c":
                        return @"\";
                    default:
                        return m.Groups[0].Value;
                }
            }

            return re.Replace(username, Evaluator);
        }

        private string BuildJid()
        {
            var builder = new StringBuilder();
            if (!string.IsNullOrEmpty(_user))
            {
                builder.Append(_user);
                builder.Append("@");
            }

            builder.Append(_server);

            if (!string.IsNullOrEmpty(_resource))
            {
                builder.Append("/");
                builder.Append(_resource);
            }

            _id = builder.ToString();
            return _id;
        }
    }
}
