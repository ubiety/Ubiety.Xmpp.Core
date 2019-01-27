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

namespace Ubiety.Xmpp.Core.Infrastructure.Extensions
{
    /// <summary>
    ///     Byte extension methods
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        ///     Combine two byte arrays using exclusive or
        /// </summary>
        /// <param name="original">Original array</param>
        /// <param name="comparison">Comparison array</param>
        /// <returns>Result of exclusive or operation</returns>
        public static byte[] ExclusiveOr(this byte[] original, byte[] comparison)
        {
            var result = new byte[original.Length];
            for (var i = 0; i < original.Length; ++i)
            {
                result[i] = (byte)(original[i] ^ comparison[i]);
            }

            return result;
        }
    }
}
