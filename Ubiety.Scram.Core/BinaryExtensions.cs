using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Scram
{
  internal static class BinaryExtensions
  {
    public static byte[] ExclusiveOr(this byte[] originalBytes, byte[] compareBytes)
    {
      if (originalBytes.Length != compareBytes.Length)
        throw new ArgumentException();

      var result = new byte[originalBytes.Length];
      for (var i = 0; i < originalBytes.Length; i++)
      {
        result[i] = (byte)(originalBytes[i] ^ compareBytes[i]);
      }

      return result;
    }
  }
}
