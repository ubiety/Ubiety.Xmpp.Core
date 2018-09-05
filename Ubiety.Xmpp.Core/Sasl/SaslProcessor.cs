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
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Ubiety.Xmpp.Core.Common;
using Ubiety.Xmpp.Core.Tags;

namespace Ubiety.Xmpp.Core.Sasl
{
    /// <summary>
    ///     SASL authentication processor
    /// </summary>
    public abstract class SaslProcessor
    {
        private readonly Hashtable _directives = new Hashtable();

        /// <summary>
        ///     Gets or sets the current client instance
        /// </summary>
        protected static XmppBase Client { get; set; }

        /// <summary>
        ///     Gets or sets the user <see cref="Jid"/> for the session
        /// </summary>
        protected Jid Id { get; set; }

        /// <summary>
        ///     Gets or sets the user password for the session
        /// </summary>
        protected string Password { get; set; }

        /// <summary>
        ///     Gets or sets the SASL processor directives
        /// </summary>
        /// <param name="directive">Directive to use</param>
        /// <returns>Value of the directive</returns>
        protected string this[string directive]
        {
            get => (string)_directives[directive];
            set => _directives[directive] = value;
        }

        /// <summary>
        ///     Creates a new SASL authentication processor
        /// </summary>
        /// <param name="serverTypes">Server supported authentication mechanisms</param>
        /// <param name="clientTypes">Client supported authentication mechanisms</param>
        /// <param name="xmpp"><see cref="XmppBase"/> instance</param>
        /// <returns>SASL processor of the most secure supported type</returns>
        public static SaslProcessor CreateProcessor(MechanismTypes serverTypes, MechanismTypes clientTypes, XmppBase xmpp)
        {
            Client = xmpp;

            if ((serverTypes & clientTypes & MechanismTypes.Plain) == MechanismTypes.Plain)
            {
                return new PlainProcessor();
            }

            return null;
        }

        /// <summary>
        ///     Process the next step in SASL authentication
        /// </summary>
        /// <param name="tag">Tag received from the server</param>
        /// <returns>Next tag to send</returns>
        public abstract Tag Step(Tag tag);

        /// <summary>
        ///     Initializes the SASL instance
        /// </summary>
        /// <param name="id"><see cref="Jid"/> of the user for authentication</param>
        /// <param name="password">Password for authentication</param>
        /// <returns>Tag to send to server</returns>
        public virtual Tag Initialize(Jid id, string password)
        {
            Id = id;
            Password = password;

            return default(Tag);
        }

        /// <summary>
        ///     Converts a byte array to a hexidecimal string
        /// </summary>
        /// <param name="buffer">Byte array buffer</param>
        /// <returns>Hexidecimal encoded string</returns>
        protected static string HexString(IEnumerable<byte> buffer)
        {
            var s = new StringBuilder();
            foreach (var item in buffer)
            {
                s.Append(item.ToString("x2"));
            }

            return s.ToString();
        }

        /// <summary>
        ///     Gets a random Int64
        /// </summary>
        /// <returns>Random Int64</returns>
        protected static long NextInt64()
        {
            var bytes = new byte[sizeof(long)];
            var random = new RNGCryptoServiceProvider();
            random.GetBytes(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }
    }
}
