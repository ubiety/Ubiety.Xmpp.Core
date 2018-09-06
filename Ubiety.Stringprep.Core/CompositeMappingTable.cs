using System.Collections.Generic;
using System.Linq;

namespace StringPrep
{
  internal class CompositeMappingTable : MappingTable
  {
    private readonly ICollection<IMappingTable> _mappingTables;
    public CompositeMappingTable(ICollection<IMappingTable> mappingTables)
    {
      _mappingTables = mappingTables;
    }

    public override bool HasReplacement(int value)
    {
      return _mappingTables.Any(table => table.HasReplacement(value));
    }

    public override int[] GetReplacement(int value)
    {
      return _mappingTables.FirstOrDefault(table => table.HasReplacement(value))?.GetReplacement(value);
    }
  }
}
