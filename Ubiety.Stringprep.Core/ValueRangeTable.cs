using System;

namespace StringPrep
{
  public class ValueRangeTable : IValueRangeTable
  {
    public static IValueRangeTable Create(params int[][] baseTables)
    {
      return Build(baseTables).Compile();
    }

    public static IValueRangeTableBuilder Build(params int[][] baseTables)
    {
      return new ValueRangeTableBuilder(baseTables);
    }

    private readonly int[] _valueRanges;
    private readonly int _length;

    internal ValueRangeTable(int[] valueRanges)
    {
      if (valueRanges.Length % 2 != 0) throw new ArgumentException("Not a value range table", nameof(valueRanges));
      _valueRanges = valueRanges;
      _length = valueRanges.Length / 2;
    }

    public bool Contains(int value)
    {
      var l = 0;
      var r = _length - 1;
      while (l <= r)
      {
        var m = (int)Math.Floor((double)(l + r) / 2);

        var lowValue = _valueRanges[m * 2];
        var highValue = _valueRanges[m * 2 + 1];

        if (lowValue == value || highValue == value || (lowValue < value && highValue > value)) return true;
        
        if (lowValue < value)
        {
          l = m + 1;
        }
        else
        {
          r = m - 1;
        }
      }

      return false;
    }
  }
}
