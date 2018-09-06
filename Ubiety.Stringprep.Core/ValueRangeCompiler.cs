using System;
using System.Collections.Generic;
using System.Linq;

namespace StringPrep
{
  internal static class ValueRangeCompiler
  {
    public static int[] Compile(int[][] baseTables, int[] inclusions, int[] removals)
    {
      foreach (var table in baseTables)
      {
        Sort(table);
      }

      Sort(inclusions);
      Sort(removals);

      return DoRemove(DoReduce(DoInclude(DoCombine(baseTables), inclusions)), removals).ToArray();
    }

    private static void Sort(int[] table)
    {
      CheckSanity(table);

      var l = table.Length / 2;
      var starts = new int[l];
      var ends = new int[l];

      for (var i = 0; i < table.Length; i++)
      {
        if (i % 2 == 0) starts[i / 2] = table[i];
        else ends[i / 2] = table[i];
      }

      Array.Sort(starts, ends);
      for (var i = 0; i < table.Length; i++)
      {
        if (i % 2 == 0) table[i] = starts[i / 2];
        else table[i] = ends[i / 2];
      }
    }

    private static void CheckSanity(int[] table)
    {
      if (table.Length % 2 != 0) throw new ArgumentException("Not a range table", nameof(table));
      for (var i = 0; i < table.Length - 1; i += 2)
      {
        if (table[i + 1] < table[i])
          throw new ArgumentException("Not a range table", nameof(table));
      }
    }

    private static List<int> DoCombine(int[][] tables)
    {
      if (tables.Length == 1) return tables[0].ToList();

      var combined = new List<int>();
      var idx = new int[tables.Length];

      while (true)
      {
        var minIdx = -1;
        var min = -1;
        for (var i = 0; i < tables.Length; i++)
        {
          if (tables[i].Length <= idx[i]) continue;
          var current = tables[i][idx[i]];
          if (min == -1 || current < min)
          {
            min = current;
            minIdx = i;
          }
        }

        if (minIdx == -1) break;
        combined.Add(tables[minIdx][idx[minIdx]]);
        combined.Add(tables[minIdx][idx[minIdx] + 1]);
        idx[minIdx] += 2;
      }

      return combined;
    }

    private static List<int> DoInclude(List<int> list, int[] inclusions)
    {
      for (var i = 0; i < inclusions.Length; i += 2)
      {
        if (inclusions[i] < list[0])
        {
          list.InsertRange(0, new[] { inclusions[i], inclusions[i + 1] });
        }
        else
        {
          var j = 0;
          var set = false;
          for (; j < list.Count; j += 2)
          {
            if (inclusions[i] <= list[j])
            {
              list.InsertRange(j, new[] { inclusions[i], inclusions[i + 1] });
              set = false;
              break;
            }
          }

          if (!set)
          {
            list.Add(inclusions[i]);
            list.Add(inclusions[i + 1]);
          }
        }
      }
      return list;
    }

    private static List<int> DoRemove(List<int> list, int[] removals)
    {
      for (var i = 0; i < removals.Length; i += 2)
      {
        for (var j = 0; j < list.Count; j += 2)
        {
          if (removals[i] == list[j])
          {
            list.RemoveAt(j--);
            CloseRemove(list, removals, ref i, ref j);
          }
          else if (removals[i] > list[j] && removals[i] < list[j + 1])
          {
            list.Insert(++j, removals[i] - 1);
            CloseRemove(list, removals, ref i, ref j);
          }
          else if (removals[i] < list[j] && (i == 0 || removals[i] > list[j - 1]))
          {
            list.RemoveAt(j--);
            CloseRemove(list, removals, ref i, ref j);
          }
        }
      }
      return list;
    }

    private static void CloseRemove(List<int> list, int[] removals, ref int i, ref int j)
    {
      for (i++; i < removals.Length; i += 2)
      {
        for (j++; j < list.Count; j += 2)
        {
          if (removals[i] == list[j])
          {
            list.RemoveAt(j);
            return;
          }
          else if (removals[i] < list[j])
          {
            list.Insert(j, removals[i] + 1);
            return;
          }
          else if (removals[i] > list[j] && (j + 1 >= list.Count || removals[i] < list[j + 1]))
          {
            list.RemoveAt(j);
            return;
          }
        }
      }
    }

    private static List<int> DoReduce(List<int> list)
    {
      var i = 1;
      while (i < list.Count - 1)
      {
        for (; i < list.Count - 1; i += 2)
        {
          if (list[i + 1] <= list[i]) // next 'start' value is either included in or abuts current range
          {
            if (list[i + 2] > list[i]) // next 'end' value should become the current end value
            {
              list[i] = list[i + 2];
            }

            list.RemoveRange(i + 1, 2);
            break;
          }
        }
      }

      return list;
    }
  }
}
