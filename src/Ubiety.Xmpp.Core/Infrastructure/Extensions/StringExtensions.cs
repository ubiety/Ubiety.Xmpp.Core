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

using System.Linq;

namespace Ubiety.Xmpp.Core.Infrastructure.Extensions
{
    /// <summary>
    /// Provides extension methods for string manipulation.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Finds the first occurrence of a given character in a string that is not escaped by a backslash.
        /// </summary>
        /// <param name="data">The input string to search.</param>
        /// <param name="token">The character to locate within the string.</param>
        /// <returns>The position of the first unescaped character or -1 if not found.</returns>
        public static int FirstUnescaped(this string data, char token)
        {
            var position = -1;
            while (position == -1 && !string.IsNullOrEmpty(data))
            {
                var index = data.IndexOf(token);
                if (index == -1)
                {
                    return -1;
                }

                if (index == 0 || data[index - 1] != '\\')
                {
                    position = index;
                }

#pragma warning disable SA1009
                data = data[(index + 1)..];
#pragma warning restore SA1009
            }

            return position;
        }

        /// <summary>
        /// Removes all whitespace characters from the given string.
        /// </summary>
        /// <param name="data">The input string to process.</param>
        /// <returns>A new string with all whitespace characters removed.</returns>
        public static string RemoveWhitespace(this string data)
        {
            return new string(data.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());
        }
    }
}
