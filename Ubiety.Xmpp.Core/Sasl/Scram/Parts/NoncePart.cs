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

namespace Ubiety.Xmpp.Core.Sasl.Scram.Parts
{
    /// <summary>
    ///     NONCE SCRAM SASL part
    /// </summary>
    internal class NoncePart : ScramPart<string>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NoncePart"/> class
        /// </summary>
        /// <param name="value">Nonce value</param>
        public NoncePart(string value)
            : base(NonceLabel, value)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NoncePart"/> class
        /// </summary>
        /// <param name="clientNonce">Client nonce value</param>
        /// <param name="serverNonce">Server nonce value</param>
        public NoncePart(string clientNonce, string serverNonce)
            : base(NonceLabel, $"{clientNonce}{serverNonce}")
        {
        }
    }
}
