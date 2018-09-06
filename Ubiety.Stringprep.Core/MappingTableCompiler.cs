using System.Collections.Generic;

namespace StringPrep
{
  internal static class MappingTableCompiler
  {
    public static IDictionary<int, int[]> Compile(IDictionary<int, int[]>[] baseTables, IDictionary<int, int[]>[] inclusions, int[] removals)
    {
      return DoRemove(DoInclude(DoCombine(baseTables), inclusions), removals);
    }

    private static IDictionary<int, int[]> DoRemove(IDictionary<int, int[]> dict, int[] removals)
    {
      foreach (var key in removals)
      {
        if (!dict.ContainsKey(key)) continue;
        dict.Remove(key);
      }
      return dict;
    }

    private static IDictionary<int, int[]> DoInclude(IDictionary<int, int[]> dict, IDictionary<int, int[]>[] inclusions)
    {
      for (var i = 0; i < inclusions.Length; i++)
      {
        foreach (var kvp in inclusions[i])
        {
          if (dict.ContainsKey(kvp.Key)) continue;
          dict.Add(kvp.Key, kvp.Value);
        }
      }
      return dict;
    }

    private static IDictionary<int, int[]> DoCombine(IDictionary<int, int[]>[] baseTables)
    {
      if (baseTables.Length == 0) return new SortedDictionary<int, int[]>();

      var combined = new SortedDictionary<int, int[]>(baseTables[0]);
      for (var i = 1; i < baseTables.Length; i++)
      {
        foreach (var kvp in baseTables[i])
        {
          if (combined.ContainsKey(kvp.Key)) continue;
          combined.Add(kvp.Key, kvp.Value);
        }
      }

      return combined;
    }
  }
}
