// <copyright file="ByteExtensions.cs" company="Dieter Lunn">
// Copyright (c) Dieter Lunn. All rights reserved.
// </copyright>

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