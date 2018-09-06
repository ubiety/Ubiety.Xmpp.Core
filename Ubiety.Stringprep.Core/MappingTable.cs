using System.Collections.Generic;

namespace StringPrep
{
  public abstract class MappingTable : IMappingTable
  {
    public static IMappingTable Create(int[] valueTable, int replacement)
    {
      return Create(valueTable, new[] { replacement });
    }

    public static IMappingTable Create(int[] valueTable, int[] replacement)
    {
      
      return Build(valueTable, replacement).Compile();
    }

    public static IMappingTable Create(params IDictionary<int, int[]>[] baseTables)
    {
      return Build(baseTables).Compile();
    }

    public static IMappingTableBuilder Build(int[] valueTable, int replacement)
    {
      return Build(valueTable, new[] { replacement });
    }

    public static IMappingTableBuilder Build(int[] valueTable, int[] replacement)
    {
      return Build().WithValueRangeTable(valueTable, replacement);
    }

    public static IMappingTableBuilder Build(params IDictionary<int, int[]>[] baseTables)
    {
      return new MappingTableBuilder(baseTables);
    }

    public abstract bool HasReplacement(int value);
    public abstract int[] GetReplacement(int value);
  }
}
