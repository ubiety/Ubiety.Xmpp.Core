namespace Ubiety.Stringprep.Core
{
    internal class ProhibitedValueStep : IPreparationProcess
    {
        private readonly IValueRangeTable _table;

        public ProhibitedValueStep(IValueRangeTable table)
        {
            _table = table;
        }

        public string Run(string input)
        {
            for (var i = 0; i < input.Length; i++)
                if (_table.Contains(input[i]))
                    throw new ProhibitedValueException(input[i]);
            return input;
        }
    }
}