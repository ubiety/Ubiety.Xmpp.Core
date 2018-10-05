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
    ///     String extension methods
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///     Get the position of the first unescaped character
        /// </summary>
        /// <param name="data">Data to read</param>
        /// <param name="token">Character to locate</param>
        /// <returns>Position of the character</returns>
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

                data = data.Substring(index + 1);
            }

            return position;
        }

        /// <summary>
        ///     Remove whitespace in a string
        /// </summary>
        /// <param name="data">Data to remove whitespace from</param>
        /// <returns>String with no whitespace</returns>
        public static string RemoveWhitespace(this string data)
        {
            return new string(data.ToCharArray().Where(c => !char.IsWhiteSpace(c)).ToArray());
        }
    }
}