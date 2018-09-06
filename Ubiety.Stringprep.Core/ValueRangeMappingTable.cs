namespace StringPrep
{
  internal class ValueRangeMappingTable : MappingTable
  {
    private readonly IValueRangeTable _valueRange;
    private readonly int[] _replacement;

    internal ValueRangeMappingTable(IValueRangeTable valueRange, int[] replacement)
    {
      _valueRange = valueRange;
      _replacement = replacement;
    }

    public override bool HasReplacement(int value)
    {
      return _valueRange.Contains(value);
    }

    public override int[] GetReplacement(int value)
    {
      return _replacement;
    }
  }
}
