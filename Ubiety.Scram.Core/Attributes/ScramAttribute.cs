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
using System.Collections.Generic;
using System.Linq;

namespace Ubiety.Scram.Core.Attributes
{
    public class ScramAttribute
    {
        protected const char AuthorizationIdentityName = 'a';
        protected const char UserName = 'n';
        protected const char MessageName = 'm';
        protected const char NonceName = 'r';
        protected const char ChannelName = 'c';
        protected const char SaltName = 's';
        protected const char IterationsName = 'i';
        protected const char ClientProofName = 'p';
        protected const char ServerSignatureName = 'v';
        protected const char ErrorName = 'e';

        public ScramAttribute(char name)
        {
            Name = name;
        }

        public char Name { get; }

        public static ICollection<ScramAttribute> ParseAll(IEnumerable<string> attributes)
        {
            return attributes.Select(Parse).ToList();
        }

        public static ScramAttribute Parse(string attribute)
        {
            var parts = attribute.Split(new[] { '=' }, 2);
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
                case AuthorizationIdentityName: return new AuthorizationIdentityAttribute(parts[1]);
                case UserName: return new UserAttribute(parts[1], true);
                case NonceName: return new NonceAttribute(parts[1]);
                case ChannelName: return new ChannelAttribute(parts[1]);
                case SaltName: return new SaltAttribute(parts[1]);
                case IterationsName: return new IterationsAttribute(parts[1]);
                case ClientProofName: return new ClientProofAttribute(parts[1]);
                case ServerSignatureName: return new ServerSignatureAttribute(parts[1]);
                case ErrorName: return new ErrorAttribute(parts[1]);
                default: return new UnknownAttribute(parts[0][0], parts[1]);
            }
        }
    }
}