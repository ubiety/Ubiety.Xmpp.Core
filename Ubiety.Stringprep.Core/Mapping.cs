using System.Collections.Generic;
using Ubiety.Stringprep.Core.Generated;

namespace Ubiety.Stringprep.Core
{
    public static class Mapping
    {
        public static readonly IDictionary<int, int[]> B_1 = Tables.B1;
        public static readonly IDictionary<int, int[]> MappedToNothing = Tables.B1;
        public static readonly IDictionary<int, int[]> B_2 = Tables.B2;
        public static readonly IDictionary<int, int[]> CaseFolding = Tables.B2;
        public static readonly IDictionary<int, int[]> B_3 = Tables.B3;
        public static readonly IDictionary<int, int[]> CaseFoldingWithoutNormalization = Tables.B3;
    }
}