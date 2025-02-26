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
    /// Represents a base class for handling SASL (Simple Authentication and Security Layer)
    /// authentication mechanisms in the XMPP protocol.
    /// </summary>
    public abstract class SaslProcessor
    {
        private readonly Hashtable _directives = new ();

        /// <summary>
        /// Gets or sets the XMPP client instance used by the SASL processors.
        /// </summary>
        protected static XmppBase Client { get; set; }

        /// <summary>
        ///     Gets or sets the user <see cref="Jid" /> for the session.
        /// </summary>
        protected Jid Id { get; set; }

        /// <summary>
        ///     Gets or sets the user password for the session.
        /// </summary>
        protected string Password { get; set; }

        /// <summary>
        /// Gets or sets the value associated with the specified directive key in the SASL processor.
        /// </summary>
        /// <param name="directive">The key representing a specific directive.</param>
        /// <returns>
        /// The value associated with the specified directive key.
        /// </returns>
        protected string this[string directive]
        {
            get => (string)_directives[directive];
            set => _directives[directive] = value;
        }

        /// <summary>
        /// Creates a SASL processor based on the most secure authentication mechanism
        /// supported by both the server and the client.
        /// </summary>
        /// <param name="serverTypes">Authentication mechanisms supported by the server.</param>
        /// <param name="clientTypes">Authentication mechanisms supported by the client.</param>
        /// <param name="xmpp">Instance of the XmppBase object.</param>
        /// <returns>A SASL processor for the most secure supported authentication mechanism, or null if no mechanism is supported.</returns>
        public static SaslProcessor CreateProcessor(
            MechanismTypes serverTypes,
            MechanismTypes clientTypes,
            XmppBase xmpp)
        {
            Client = xmpp;

            if ((serverTypes & clientTypes & MechanismTypes.Scram) == MechanismTypes.Scram)
            {
                return new ScramProcessor(false);
            }

            if ((serverTypes & clientTypes & MechanismTypes.DigestMd5) == MechanismTypes.DigestMd5)
            {
                return new Md5Processor();
            }

            if ((serverTypes & clientTypes & MechanismTypes.Plain) == MechanismTypes.Plain)
            {
                return new PlainProcessor();
            }

            return null;
        }

        /// <summary>
        /// Executes a single step in the SASL authentication process using the provided server tag.
        /// </summary>
        /// <param name="tag">Tag received from the server to process.</param>
        /// <returns>The next tag to send to the server as part of the authentication process.</returns>
        public abstract Tag Step(Tag tag);

        /// <summary>
        /// Initializes the SASL processor with the user's credentials.
        /// </summary>
        /// <param name="id">The <see cref="Jid"/> of the user for authentication.</param>
        /// <param name="password">The password of the user for authentication.</param>
        /// <returns>A <see cref="Tag"/> representing the SASL authentication data to send to the server.</returns>
        public virtual Tag Initialize(Jid id, string password)
        {
            Id = id;
            Password = password;

            return null;
        }

        /// <summary>
        /// Converts a sequence of bytes into a hexadecimal string representation.
        /// </summary>
        /// <param name="buffer">The byte sequence to be converted.</param>
        /// <returns>A string containing the hexadecimal representation of the specified byte sequence.</returns>
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
        /// Generates a random Int64 value using a cryptographically secure random number generator.
        /// </summary>
        /// <returns>A randomly generated Int64 value.</returns>
        protected static long NextInt64()
        {
            var bytes = new byte[sizeof(long)];

            RandomNumberGenerator.Fill(bytes);
            return BitConverter.ToInt64(bytes, 0);
        }

        /// <summary>
        /// Generates a unique string GUID to be used as a NONCE.
        /// </summary>
        /// <returns>A string representing the GUID for the NONCE.</returns>
        protected static string CreateNonce()
        {
            return Guid.NewGuid().ToString();
        }
    }
}
