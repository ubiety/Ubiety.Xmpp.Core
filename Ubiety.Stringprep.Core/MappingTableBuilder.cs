using System;
using System.Collections.Generic;
using System.Linq;

namespace StringPrep
{
  internal class MappingTableBuilder : IMappingTableBuilder
  {
    private readonly List<IDictionary<int, int[]>> _baseTables;
    private readonly List<Tuple<int[], int[]>> _valueRangeBaseTables;
    private readonly List<IDictionary<int, int[]>> _inclusions;
    private readonly List<int> _removals;

    public MappingTableBuilder(params IDictionary<int, int[]>[] baseTables)
    {
      _baseTables = baseTables.ToList();
      _valueRangeBaseTables = new List<Tuple<int[], int[]>>();
      _inclusions = new List<IDictionary<int, int[]>>();
      _removals = new List<int>();
    }

    public IMappingTableBuilder WithValueRangeTable(int[] values, int replacement)
    {
      return WithValueRangeTable(values, new[] { replacement });
    }
    
    public IMappingTableBuilder WithValueRangeTable(int[] values, int[] replacement)
    {
      _valueRangeBaseTables.Add(new Tuple<int[], int[]>(values, replacement));
      return this;
    }

    public IMappingTableBuilder WithMappingTable(IDictionary<int, int[]> table)
    {
      _baseTables.Add(table);
      return this;
    }

    public IMappingTableBuilder Include(IDictionary<int, int[]> include)
    {
      _inclusions.Add(include);
      return this;
    }

    public IMappingTableBuilder Remove(int remove)
    {
      _removals.Add(remove);
      return this;
    }

    public IMappingTable Compile()
    {
      if (!_baseTables.Any() && !_inclusions.Any() && !_valueRangeBaseTables.Any()) throw new InvalidOperationException("At least one table must be provided");
      var mappingTables = new List<IMappingTable>()
      {
        new DictionaryMappingTable(MappingTableCompiler.Compile(_baseTables.ToArray(), _inclusions.ToArray(), _removals.ToArray()))
      };
      
      foreach (var t in _valueRangeBaseTables)
      {
        var valueRangeTable = ValueRangeCompiler.Compile(new[] {t.Item1}, new int[0], _removals.ToArray());
        mappingTables.Add(new ValueRangeMappingTable(new ValueRangeTable(valueRangeTable), t.Item2));
      }

      return new CompositeMappingTable(mappingTables);
    }
  }
}
