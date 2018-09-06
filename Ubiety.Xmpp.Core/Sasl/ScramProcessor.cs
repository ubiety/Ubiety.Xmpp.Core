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
using System.Security.Cryptography;
using System.Text;
using Gnu.Inet.Encoding;
using StringPrep;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Stringprep;
using Ubiety.Xmpp.Core.Tags;
using Ubiety.Xmpp.Core.Tags.Sasl;

namespace Ubiety.Xmpp.Core.Sasl
{
    /// <summary>
    ///     SCRAM-SHA-1 SASL Processor
    /// </summary>
    public class ScramProcessor : SaslProcessor
    {
        private readonly IPreparationProcess saslprep = SaslprepProfile.Create();
        private readonly Encoding _encoding = Encoding.UTF8;
        private string _nonce;
        private string _clientFirst;
        private string _clientFinal;
        private byte[] _serverFirst;
        private byte[] _salt;
        private int _iterations;

        /// <summary>
        ///     Initializes the SASL processor
        /// </summary>
        /// <param name="id"><see cref="Jid"/> of the user for the session</param>
        /// <param name="password">Password of the user</param>
        /// <returns>Next tag to send to the server</returns>
        public override Tag Initialize(Jid id, string password)
        {
            base.Initialize(id, password);

            _nonce = NextInt64().ToString();
            var message = $"n,,n={Id.User},r={_nonce}";

            _clientFirst = message.Substring(3);

            var auth = Client.Registry.GetTag<Auth>(Auth.XmlName);
            auth.MechanismType = MechanismTypes.Scram;
            auth.Bytes = _encoding.GetBytes(message);

            return auth;
        }

        /// <summary>
        ///     Process the next SASL step
        /// </summary>
        /// <param name="tag">Tag received from the server</param>
        /// <returns>Next tag to send to the server</returns>
        public override Tag Step(Tag tag)
        {
            switch (tag)
            {
                case Challenge c:
                    return ProcessChallenge(c);
                case Success s:
                    break;
                case Failure f:
                    return f;
                default:
                    return default(Tag);
            }
        }

        private Tag ProcessChallenge(Challenge tag)
        {
            _serverFirst = tag.Bytes;
            var response = _encoding.GetString(tag.Bytes);

            var tokens = response.Split(',');
            var snonce = tokens[0].Substring(2);
            var prefix = snonce.Substring(0, _nonce.Length);
            if (string.CompareOrdinal(prefix, _nonce) != 0)
            {
                return default(Tag);
            }

            var salt = tokens[1].Substring(2);
            _salt = Convert.FromBase64String(salt);

            var i = tokens[2].Substring(2);
            _iterations = int.Parse(i);

            _clientFinal = $"c=biws,r={snonce}";

            CalculateProofs();
        }

        private void CalculateProofs()
        {
            var hmac = new HMACSHA1();
            SHA1 hash = new SHA1CryptoServiceProvider();

            var saltedPassword = Hi();
        }

        private byte[] Hi()
        {
            var prev = new byte[20];
            var password = _encoding.GetBytes(saslprep.Run(Password));

            var key = new byte[_salt.Length + 4];
            Array.Copy(_salt, key, _salt.Length);
            byte[] g = { 0, 0, 0, 1 };
            Array.Copy(g, 0, key, _salt.Length, 4);

            var hmac = new HMACSHA1(password);
            var result = hmac.ComputeHash(key);
            Array.Copy(result, prev, result.Length);

            for (int i = 0; i < _iterations; i++)
            {
                var temp = hmac.ComputeHash(prev);
                for (int j = 0; j < temp.Length; j++)
                {
                    result[j] ^= temp[j];
                }

                Array.Copy(temp, prev, temp.Length);
            }

            return result;
        }
    }
}
