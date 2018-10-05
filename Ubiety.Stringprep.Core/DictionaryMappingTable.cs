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

using System.Collections.Generic;

namespace Ubiety.Stringprep.Core
{
    /// <summary>
    ///     Dictionary mapping table
    /// </summary>
    internal class DictionaryMappingTable : MappingTable
    {
        private readonly SortedList<int, int[]> _mappings;

        /// <summary>
        ///     Initializes a new instance of the <see cref="DictionaryMappingTable" /> class
        /// </summary>
        /// <param name="values">Mapping values</param>
        internal DictionaryMappingTable(IDictionary<int, int[]> values)
        {
            _mappings = new SortedList<int, int[]>(values);
        }

        /// <summary>
        ///     Does the value have a replacement
        /// </summary>
        /// <param name="value">Value to replace</param>
        /// <returns>A value indicating whether or not it can be replaced</returns>
        public override bool HasReplacement(int value)
        {
            return _mappings.ContainsKey(value);
        }

        /// <summary>
        ///     Gets the replacement value
        /// </summary>
        /// <param name="value">Value to replace</param>
        /// <returns>Replacement value</returns>
        public override int[] GetReplacement(int value)
        {
            return _mappings[value];
        }
    }
}