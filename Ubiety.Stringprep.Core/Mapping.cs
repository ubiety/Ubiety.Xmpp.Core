using System.Collections.Generic;

namespace StringPrep
{
  public static class Mapping
  {
    public static readonly IDictionary<int, int[]> B_1 = Tables.B_1;
    public static readonly IDictionary<int, int[]> MappedToNothing = Tables.B_1;
    public static readonly IDictionary<int, int[]> B_2 = Tables.B_2;
    public static readonly IDictionary<int, int[]> CaseFolding = Tables.B_2;
    public static readonly IDictionary<int, int[]> B_3 = Tables.B_3;
    public static readonly IDictionary<int, int[]> CaseFoldingWithoutNormalization = Tables.B_3;
  }
}
