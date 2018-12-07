using System.Collections.Generic;

namespace Ubiety.Stringprep.Core
{
    /// <summary>
    ///     Mapping table builder interface
    /// </summary>
    public interface IMappingTableBuilder
    {
        /// <summary>
        ///     Builds a mapping with a value range
        /// </summary>
        /// <param name="values">Values array</param>
        /// <param name="replacement">Replacement value</param>
        /// <returns>Mapping builder</returns>
        IMappingTableBuilder WithValueRangeTable(int[] values, int replacement);

        /// <summary>
        ///     Builds a mapping with a value range
        /// </summary>
        /// <param name="values">Values array</param>
        /// <param name="replacement">Replacement array</param>
        /// <returns>Mapping builder</returns>
        IMappingTableBuilder WithValueRangeTable(int[] values, int[] replacement);

        /// <summary>
        ///     Builds a mapping with a mapping table
        /// </summary>
        /// <param name="table">Mapping table</param>
        /// <returns>Mapping builder</returns>
        IMappingTableBuilder WithMappingTable(IDictionary<int, int[]> table);

        IMappingTableBuilder Include(IDictionary<int, int[]> include);

        IMappingTableBuilder Remove(int remove);

        IMappingTable Compile();
    }
}