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

namespace Ubiety.Xmpp.Core.Sasl.Scram.Parts
{
    /// <summary>
    ///     SCRAM client proof part
    /// </summary>
    internal class ClientProofPart : ScramPart<byte[]>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientProofPart"/> class
        /// </summary>
        /// <param name="value">Client proof value</param>
        public ClientProofPart(string value)
            : base(ClientProofLabel, Convert.FromBase64String(value))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ClientProofPart"/> class
        /// </summary>
        /// <param name="value">Client proof calue</param>
        public ClientProofPart(byte[] value)
            : base(ClientProofLabel, value)
        {
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Label}={Convert.ToBase64String(Value)}";
        }
    }
}
