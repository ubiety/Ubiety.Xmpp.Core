using System;
using System.Text;

namespace Ubiety.Stringprep.Core
{
    internal class MappingStep : IPreparationProcess
    {
        private readonly IMappingTable _table;

        public MappingStep(IMappingTable table)
        {
            _table = table;
        }

        public string Run(string input)
        {
            var sb = new StringBuilder();
            foreach (var c in input)
                if (_table.HasReplacement(c))
                    foreach (var r in _table.GetReplacement(c))
                        sb.Append(Convert.ToChar(r));
                else
                    sb.Append(c);
            return sb.ToString();
        }
    }
}