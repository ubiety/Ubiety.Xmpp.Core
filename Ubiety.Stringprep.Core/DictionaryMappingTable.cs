using System.Collections.Generic;

namespace StringPrep
{
  internal class DictionaryMappingTable : MappingTable
  {
    private readonly SortedList<int, int[]> _mappings;

    internal DictionaryMappingTable(IDictionary<int, int[]> values)
    {
      _mappings = new SortedList<int, int[]>(values);
    }

    public override bool HasReplacement(int value)
    {
      return _mappings.ContainsKey(value);
    }

    public override int[] GetReplacement(int value)
    {
      return _mappings[value];
    }
  }
}
