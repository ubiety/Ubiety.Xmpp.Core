using System.Collections.Generic;
using System.Linq;

namespace StringPrep
{
    /// <summary>
    ///     Composite mapping table
    /// </summary>
    internal class CompositeMappingTable : MappingTable
    {
        private readonly ICollection<IMappingTable> _mappingTables;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CompositeMappingTable"/> class
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