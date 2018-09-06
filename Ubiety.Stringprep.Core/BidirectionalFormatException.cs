using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringPrep
{
  public class BidirectionalFormatException : Exception
  {
    public BidirectionalFormatException(string message) : base(message)
    {
    }
  }
}
