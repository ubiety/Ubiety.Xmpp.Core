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
using System.Collections.Generic;
using System.Linq;

namespace Ubiety.Xmpp.Core.Sasl.Scram.Parts
{
    /// <summary>
    ///     SCRAM message part
    /// </summary>
    internal class ScramPart
    {
        /// <summary>
        ///     Authorization identity label
        /// </summary>
        protected const char AuthorizationIdentityLabel = 'a';

        /// <summary>
        ///     Username label
        /// </summary>
        protected const char UsernameLabel = 'n';

        /// <summary>
        ///     Message label
        /// </summary>
        protected const char MessageLabel = 'm';

        /// <summary>
        ///     Nonce label
        /// </summary>
        protected const char NonceLabel = 'r';

        /// <summary>
        ///     Channel label
        /// </summary>
        protected const char ChannelLabel = 'c';

        /// <summary>
        ///     Salt label
        /// </summary>
        protected const char SaltLabel = 's';

        /// <summary>
        ///     Iteration label
        /// </summary>
        protected const char IterationLabel = 'i';

        /// <summary>
        ///     Client proof label
        /// </summary>
        protected const char ClientProofLabel = 'p';

        /// <summary>
        ///     Server signature label
        /// </summary>
        protected const char ServerSignatureLabel = 'v';

        /// <summary>
        ///     Error label
        /// </summary>
        protected const char ErrorLabel = 'e';

        /// <summary>
        ///     Initializes a new instance of the <see cref="ScramPart"/> class
        /// </summary>
        /// <param name="label">SCRAM label of the part</param>
        public ScramPart(char label)
        {
            Label = label;
        }

        /// <summary>
        ///     Gets the SCRAM part label
        /// </summary>
        public char Label { get; }

        /// <summary>
        ///     Parse all parts
        /// </summary>
        /// <param name="parts">Collection of parts to parse</param>
        /// <returns>Collection of part objects</returns>
        public static ICollection<ScramPart> ParseAll(IEnumerable<string> parts)
        {
            return parts.Select(Parse).ToList();
        }

        /// <summary>
        ///     Parse a part into its object form
        /// </summary>
        /// <param name="part">String version of the part</param>
        /// <returns>Object version of the part</returns>
        public static ScramPart Parse(string part)
        {
            var parts = part.Split(new[] { '=' }, 2);
            if (parts.Length != 2)
            {
                throw new FormatException();
            }

            if (parts[0].Length > 1)
            {
                throw new FormatException();
            }

            switch (parts[0][0])
            {
                case AuthorizationIdentityLabel:
                    return new AuthorizationIdentityPart(parts[1]);
                case UsernameLabel:
                    return new UsernamePart(parts[1]);
                case NonceLabel:
                    return new NoncePart(parts[1]);
                case SaltLabel:
                    return new SaltPart(parts[1]);
                case IterationLabel:
                    return new IterationPart(parts[1]);
                default:
                    return default(ScramPart);
            }
        }
    }
}
