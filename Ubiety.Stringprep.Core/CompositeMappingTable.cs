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
using System.Linq;

namespace Ubiety.Stringprep.Core
{
    /// <summary>
    ///     Composite mapping table
    /// </summary>
    internal class CompositeMappingTable : MappingTable
    {
        private readonly ICollection<IMappingTable> _mappingTables;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CompositeMappingTable" /> class
        /// </summary>
        /// <param name="mappingTables">Mapping tables</param>
        public CompositeMappingTable(ICollection<IMappingTable> mappingTables)
        {
            _mappingTables = mappingTables;
        }

        /// <summary>
        ///     Does the value have a replacement
        /// </summary>
        /// <param name="value">Value to find replacement for</param>
        /// <returns>A value indicating whether there is a replacement or not</returns>
        public override bool HasReplacement(int value)
        {
            return _mappingTables.Any(table => table.HasReplacement(value));
        }

        /// <summary>
        ///     Gets the replacement for the value
        /// </summary>
        /// <param name="value">Value to get the replacement for</param>
        /// <returns>Replacement for the value</returns>
        public override int[] GetReplacement(int value)
        {
            return _mappingTables.FirstOrDefault(table => table.HasReplacement(value))?.GetReplacement(value);
        }
    }
}