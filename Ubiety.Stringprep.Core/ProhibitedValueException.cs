using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringPrep
{
  public class ProhibitedValueException : Exception
  {
    public ProhibitedValueException(char prohibited) : base($"The string contains the prohibited value: '{prohibited}'")
    {
    }
  }
}
