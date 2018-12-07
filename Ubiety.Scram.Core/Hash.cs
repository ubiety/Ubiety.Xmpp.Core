// This is free and unencumbered software released into the public domain.
//
// Anyone is free to copy, modify, publish, use, compile, sell, or
// distribute this software, either in source code form or as a compiled
// binary, for any purpose, commercial or non-commercial, and by any
// means.
//
// In jurisdictions that recognize copyright laws, the author or authors
// of this software dedicate any and all copyright interest in the
// software to the public domain. We make this dedication for the benefit
// of the public at large and to the detriment of our heirs and
// successors. We intend this dedication to be an overt act of
// relinquishment in perpetuity of all present and future rights to this
// software under copyright law.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS BE LIABLE FOR ANY CLAIM, DAMAGES OR
// OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE,
// ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR
// OTHER DEALINGS IN THE SOFTWARE.
//
// For more information, please refer to <http://unlicense.org/>

using System;
using System.Linq;
using System.Security.Cryptography;

namespace Ubiety.Scram.Core
{
    public class Hash
    {
        private readonly HashAlgorithm _hashAlgorithm;
        private readonly Func<byte[], HMAC> _hmacFactory;

        private Hash(HashAlgorithm algorithm, Func<byte[], HMAC> hmacFactory)
        {
            _hashAlgorithm = algorithm;
            _hmacFactory = hmacFactory;
        }

        public static Hash Sha1()
        {
            return new Hash(new SHA1Managed(), GetHmacSha1);
        }

        public static Hash Sha256()
        {
            return new Hash(new SHA256Managed(), GetHmacSha256);
        }

        public byte[] ComputeHash(byte[] value)
        {
            return _hashAlgorithm.ComputeHash(value);
        }

        public byte[] ComputeHash(byte[] value, byte[] key)
        {
            var hmacAlgorithm = _hmacFactory(key);
            return hmacAlgorithm.ComputeHash(value);
        }

        public byte[] ComputeHash(byte[] value, byte[] salt, int iterations)
        {
            var one = BitConverter.GetBytes(1);
            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(one);
            }

            var completeSalt = salt.Concat(one).ToArray();
            var iteration = ComputeHash(value, completeSalt);
            var final = iteration;

            for (var i = 1; i < iterations; i++)
            {
                var temp = ComputeHash(iteration, value);
                final = final.ExclusiveOr(temp);
                iteration = temp;
            }

            return final;
        }

        private static HMAC GetHmacSha1(byte[] key)
        {
            return new HMACSHA1(key);
        }

        private static HMAC GetHmacSha256(byte[] key)
        {
            return new HMACSHA256(key);
        }
    }
}