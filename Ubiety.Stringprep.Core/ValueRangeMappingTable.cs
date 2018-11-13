namespace Ubiety.Stringprep.Core
{
    internal class ValueRangeMappingTable : MappingTable
    {
        private readonly int[] _replacement;
        private readonly IValueRangeTable _valueRange;

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