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
using Gnu.Inet.Encoding;

namespace Ubiety.Xmpp.Core.Common
{
    /// <inheritdoc />
    /// <summary>
    ///     JID class
    /// </summary>
    public sealed class Jid : IEquatable<Jid>
    {
        private string _id;
        private string _resource;
        private string _server;
        private string _user;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Jid" /> class
        /// </summary>
        /// <param name="id">String version of the ID</param>
        public Jid(string id)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Jid"/> class
        /// </summary>
        /// <param name="username">Username of the user</param>
        /// <param name="server">XMPP server of the user</param>
        /// <param name="resource">Server resource</param>
        public Jid(string username, string server, string resource = "")
        {
            User = username ?? throw new ArgumentNullException(nameof(username));
            Server = server ?? throw new  ArgumentNullException(nameof(server));
            Resource = resource;
        }

        /// <summary>
        ///     Gets the JID resource
        /// </summary>
        public string Resource
        {
            get => _resource;
            private set => _resource = value is null ? null : Stringprep.ResourcePrep(value);
        }

        /// <summary>
        ///     Gets the server of the JID
        /// </summary>
        public string Server
        {
            get => _server;
            private set => _server = value is null ? null : Stringprep.NamePrep(value);
        }

        /// <summary>
        ///     Gets the user name of the JID
        /// </summary>
        public string User
        {
            get => Unescape();
            private set
            {
                var temp = Escape(value);
                _user = Stringprep.NamePrep(temp);
            }
        }

        /// <summary>
        ///     Gets or sets the id as a string
        /// </summary>
        public string Id
        {
            get => _id ?? BuildJid();
            set => Parse(value);
        }

        /// <inheritdoc />
        public bool Equals(Jid other)
        {
            return Id.Equals(other.Id);
        }

        /// <summary>
        ///     Implicitly converts a string into a <see cref="Jid" />
        /// </summary>
        /// <param name="id">String version of the ID</param>
        public static implicit operator Jid(string id)
        {
            return new Jid(id);
        }

        /// <summary>
        ///     Implicitly converts a <see cref="Jid" /> to a string
        /// </summary>
        /// <param name="id"><see cref="Jid" /> of the ID</param>
        public static implicit operator string(Jid id)
        {
            return id.Id;
        }

        /// <summary>
        ///     Compares equality of one <see cref="Jid" /> to another
        /// </summary>
        /// <param name="one">First <see cref="Jid" /></param>
        /// <param name="two">Second <see cref="Jid" /></param>
        /// <returns>True if the Jids are equal</returns>
        public static bool operator ==(Jid one, Jid two)
        {
            return one != null && one.Equals(two);
        }

        /// <summary>
        ///     Compares the inequality of one <see cref="Jid" /> to another
        /// </summary>
        /// <param name="one">First <see cref="Jid" /></param>
        /// <param name="two">Second <see cref="Jid" /></param>
        /// <returns>True if the Jids are not equal</returns>
        public static bool operator !=(Jid one, Jid two)
        {
            return one != null && !one.Equals(two);
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

                hash = hash * 23 + User.GetHashCode();
                hash = hash * 23 + Server.GetHashCode();
                hash = hash * 23 + Resource.GetHashCode();

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
            var u = new StringBuilder();
            var count = 0;

            foreach (var c in user)
            {
                switch (c)
                {
                    case ' ':
                        if (count == 0 || count == user.Length - 1)
                            throw new FormatException("Username cannot start or end with a space");

                        u.Append(@"\20");
                        break;
                    case '"':
                        u.Append(@"\22");
                        break;
                    case '&':
                        u.Append(@"\26");
                        break;
                    case '\'':
                        u.Append(@"\27");
                        break;
                    case '/':
                        u.Append(@"\2f");
                        break;
                    case ':':
                        u.Append(@"\3a");
                        break;
                    case '<':
                        u.Append(@"\3c");
                        break;
                    case '>':
                        u.Append(@"\3e");
                        break;
                    case '@':
                        u.Append(@"\40");
                        break;
                    case '\\':
                        u.Append(@"\5c");
                        break;
                    default:
                        u.Append(c);
                        break;
                }

                count++;
            }

            return u.ToString();
        }

        private string Unescape()
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

            var u = re.Replace(_user, Evaluator);

            return u;
        }

        private void Parse(string value)
        {
            var at = value.IndexOf("@", StringComparison.Ordinal);
            var slash = value.IndexOf("/", StringComparison.Ordinal);

            if (at == -1)
            {
                if (slash == -1)
                {
                    Server = value;
                }
                else
                {
                    Server = value.Substring(0, slash);
                    Resource = value.Substring(slash + 1);
                }
            }
            else
            {
                if (slash == -1)
                {
                    User = value.Substring(0, at);
                    Server = value.Substring(at + 1);
                }
                else
                {
                    if (at < slash)
                    {
                        User = value.Substring(0, at);
                        Server = value.Substring(at + 1, slash - at - 1);
                        Resource = value.Substring(slash + 1);
                    }
                    else
                    {
                        Server = value.Substring(0, slash);
                        Resource = value.Substring(slash + 1);
                    }
                }
            }
        }

        private string BuildJid()
        {
            var builder = new StringBuilder();
            if (_user != null)
            {
                builder.Append(_user);
                builder.Append("@");
            }

            builder.Append(_server);

            if (_resource != null)
            {
                builder.Append("/");
                builder.Append(_resource);
            }

            _id = builder.ToString();
            return _id;
        }
    }
}