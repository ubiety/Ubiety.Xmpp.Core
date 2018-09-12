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
    ///     SCRAM SASL part
    /// </summary>
    /// <typeparam name="T">Type of the part value</typeparam>
    internal class ScramPart<T> : ScramPart
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ScramPart{T}" /> class
        /// </summary>
        /// <param name="label">Part label</param>
        /// <param name="value">Part value</param>
        public ScramPart(char label, T value)
            : base(label)
        {
            Value = value;
        }

        /// <summary>
        ///     Gets the part value
        /// </summary>
        public T Value { get; }

        /// <inheritdoc />
        public override string ToString()
        {
            return $"{Label}={Value}";
        }
    }
}