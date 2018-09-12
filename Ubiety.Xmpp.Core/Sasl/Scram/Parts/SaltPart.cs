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
    ///     SASL SCRAM salt part
    /// </summary>
    internal class SaltPart : ScramPart<byte[]>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SaltPart"/> class
        /// </summary>
        /// <param name="value">Salt value</param>
        public SaltPart(byte[] value)
            : base(SaltLabel, value)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SaltPart"/> class
        /// </summary>
        /// <param name="value">String version of the salt</param>
        public SaltPart(string value)
            : base(SaltLabel, Convert.FromBase64String(value))
        {
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{Label}={Convert.ToBase64String(Value)}";
        }
    }
}
