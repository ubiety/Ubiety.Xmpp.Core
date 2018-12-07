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
using Ubiety.Scram.Core.Attributes;

namespace Ubiety.Scram.Core.Model
{
    public class ServerFirstMessage
    {
        public ServerFirstMessage(int iterations, string nonce, byte[] salt)
        {
            Iterations = new IterationsAttribute(iterations);
            Nonce = new NonceAttribute(nonce);
            Salt = new SaltAttribute(salt);
        }

        private ServerFirstMessage(IterationsAttribute iterations, NonceAttribute nonce, SaltAttribute salt)
        {
            Iterations = iterations;
            Nonce = nonce;
            Salt = salt;
        }

        public ScramAttribute<int> Iterations { get; }

        public ScramAttribute<string> Nonce { get; }

        public ScramAttribute<byte[]> Salt { get; }

        public static ServerFirstMessage ParseResponse(string response)
        {
            var parts = ScramAttribute.ParseAll(response.Split(','));

            var errors = parts.OfType<ErrorAttribute>();
            if (errors.Any())
            {
                throw new InvalidOperationException();
            }

            var iterations = parts.OfType<IterationsAttribute>().ToList();
            var nonces = parts.OfType<NonceAttribute>().ToList();
            var salts = parts.OfType<SaltAttribute>().ToList();

            if (!iterations.Any() || !nonces.Any() || !salts.Any())
            {
                throw new InvalidOperationException();
            }

            return new ServerFirstMessage(iterations.First(), nonces.First(), salts.First());
        }
    }
}