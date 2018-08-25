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

namespace Ubiety.Xmpp.Core.Infrastructure.Extensions
{
    /// <summary>
    ///     Byte extension methods
    /// </summary>
    public static class ByteExtensions
    {
        /// <summary>
        ///     Trims null byte values from the end of an array
        /// </summary>
        /// <param name="bytes">Byte array to trim</param>
        /// <returns>Trimmed array</returns>
        public static byte[] TrimNullBytes(this IList<byte> bytes)
        {
            if (bytes.Count <= 1) return null;

            var lastByte = bytes.Count - 1;
            while (bytes[lastByte] == 0x00)
            {
                lastByte--;
                if (lastByte < 0) break;

                var newArray = new byte[lastByte + 1];
                for (var i = 0; i < lastByte + 1; i++) newArray[i] = bytes[i];

                return newArray;
            }

            return null;
        }

        /// <summary>
        ///     Clears a byte array
        /// </summary>
        /// <param name="data">Byte array to clear</param>
        public static void Clear(this byte[] data)
        {
            Array.Clear(data, 0, data.Length);
        }
    }
}