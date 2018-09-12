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
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Tags;
using Ubiety.Xmpp.Core.Tags.Sasl;

namespace Ubiety.Xmpp.Core.Sasl
{
    /// <summary>
    ///     MD5 SASL processor
    /// </summary>
    public class Md5Processor : SaslProcessor
    {
        private readonly Regex _csv = new Regex(
            @"(?<tag>[^=]+)=(?:(?<data>[^,""]+)|(?:""(?<data>[^""]*)"")),?",
            RegexOptions.Compiled | RegexOptions.ExplicitCapture);

        private readonly Encoding _encoding = Encoding.UTF8;
        private readonly MD5CryptoServiceProvider _md5 = new MD5CryptoServiceProvider();
        private string _cnonce;
        private string _digestUri;

        private int _nonceCount;
        private string _responseHash;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Md5Processor" /> class
        /// </summary>
        public Md5Processor()
        {
            _nonceCount = 0;
        }

        /// <summary>
        ///     Initializes the SASL processor
        /// </summary>
        /// <param name="id"><see cref="Jid" /> of the user to authenticate</param>
        /// <param name="password">Password to use for authentication</param>
        /// <returns>Next tag to send to the server</returns>
        public override Tag Initialize(Jid id, string password)
        {
            base.Initialize(id, password);

            var auth = Client.Registry.GetTag<Auth>(Auth.XmlName);
            auth.MechanismType = MechanismTypes.DigestMd5;
            return auth;
        }

        /// <summary>
        ///     Process the next SASL step
        /// </summary>
        /// <param name="tag">Tag from the server</param>
        /// <returns>Next tag to send to the server</returns>
        public override Tag Step(Tag tag)
        {
            switch (tag)
            {
                case Response s:
                    PopulateDirectives(s);
                    return s;
                case Failure f:
                    return f;
                default:
                    PopulateDirectives(tag);
                    var response = Client.Registry.GetTag<Auth>(Auth.XmlName);
                    if (this["rspauth"] != null)
                    {
                        return response;
                    }

                    GenerateResponseHash();
                    response.Bytes = GenerateResponse();

                    return response;
            }
        }

        private void PopulateDirectives(Tag tag)
        {
            var col = _csv.Matches(_encoding.GetString(tag.Bytes));

            foreach (Match item in col)
            {
                this[item.Groups["tag"].Value] = item.Groups["data"].Value;
            }

            _digestUri = this["realm"] != null ? $"xmpp/{this["realm"]}" : $"xmpp/{Id.Server}";
        }

        private void GenerateResponseHash()
        {
            var encoding = new ASCIIEncoding();

            _cnonce = HexString(encoding.GetBytes($"{NextInt64()}:{Id.User}:{Password}")).ToLower();

            _nonceCount++;

            var h1 = _md5.ComputeHash(encoding.GetBytes($"{Id.User}:{this["realm"]}:{Password}"));
            var a1 = encoding.GetBytes(
                $":{this["nonce"]}:{_cnonce}{(string.IsNullOrEmpty(this["authzid"]) ? string.Empty : ":" + this["authzid"])}");

            var a1Final = _md5.ComputeHash(h1.Concat(a1).ToArray());

            var a2 = $"AUTHENTICATE:{_digestUri}";
            if (string.Compare(this["qop"], "auth", StringComparison.Ordinal) != 0)
            {
                a2 += ":00000000000000000000000000000000";
            }

            var a2Final = _md5.ComputeHash(encoding.GetBytes(a2));

            var a1String = HexString(a1Final).ToLower();
            var a2String = HexString(a2Final).ToLower();

            var h3 = $"{a1String}:{this["nonce"]}:{_nonceCount}:{_cnonce}:{this["qop"]}:{a2String}";

            _responseHash = HexString(_md5.ComputeHash(encoding.GetBytes(h3))).ToLower();
        }

        private byte[] GenerateResponse()
        {
            var response =
                $"username=\"{Id.User}\",realm=\"{this["realm"]}\",nonce=\"{this["nonce"]}\",cnonce=\"{_cnonce}\",nc={_nonceCount},qop={this["qop"]},digest-uri=\"{_digestUri}\",response={_responseHash},charset={this["charset"]}";

            return _encoding.GetBytes(response);
        }
    }
}