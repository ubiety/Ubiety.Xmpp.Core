using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringPrep
{
  internal class BidirectionalStep : IPreparationProcess
  {
    private readonly IValueRangeTable _prohibitedTable;
    private readonly IValueRangeTable _ralTable;
    private readonly IValueRangeTable _lTable;

    public BidirectionalStep(IValueRangeTable prohibitedTable, IValueRangeTable ralTable, IValueRangeTable lTable)
    {
      _prohibitedTable = prohibitedTable;
      _ralTable = ralTable;
      _lTable = lTable;
    }

    public string Run(string input)
    {
      var ralProhibited = false;
      var ralFound = false;
      var ral = false;
      var l = false;
      var first = true;

      foreach (var c in input)
      {
        if (_prohibitedTable.Contains(c))
          throw new ProhibitedValueException(c);

        ralFound = _ralTable.Contains(c);
        ral = ral || ralFound;
        if (ral && ralProhibited)
          throw new BidirectionalFormatException("A character from the RandAL set was found without the string beginning with an RandAL character");

        l = l || _lTable.Contains(c);
        if (ral && l)
          throw new BidirectionalFormatException("Characters from both the RandAL and L sets were found");

        if (first)
        {
          first = false;
          ralProhibited = !ral;
        }
      }

      if (ral && !ralFound)
        throw new BidirectionalFormatException("A character from the RandAL set must be the last character in an RandAL string");

      return input;
    }
  }
}
