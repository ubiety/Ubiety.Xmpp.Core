using Ubiety.Stringprep.Core.Generated;

namespace Ubiety.Stringprep.Core
{
    public static class Prohibited
    {
        public static readonly int[] C_1_1 = Tables.C11;
        public static readonly int[] ASCIISpaceCharacters = Tables.C11;
        public static readonly int[] C_1_2 = Tables.C12;
        public static readonly int[] NonASCIISpaceCharacters = Tables.C12;
        public static readonly int[] C_2_1 = Tables.C21;
        public static readonly int[] ASCIIControlCharacters = Tables.C21;
        public static readonly int[] C_2_2 = Tables.C22;
        public static readonly int[] NonASCIIControlCharacters = Tables.C22;
        public static readonly int[] C_3 = Tables.C3;
        public static readonly int[] PrivateUseCharacters = Tables.C3;
        public static readonly int[] C_4 = Tables.C4;
        public static readonly int[] NonCharacterCodePoints = Tables.C5;
        public static readonly int[] C_5 = Tables.C5;
        public static readonly int[] SurrogateCodePoints = Tables.C5;
        public static readonly int[] C_6 = Tables.C6;
        public static readonly int[] InappropriateForPlainText = Tables.C6;
        public static readonly int[] C_7 = Tables.C7;
        public static readonly int[] InappropriateForCanonicalRepresentation = Tables.C7;
        public static readonly int[] C_8 = Tables.C8;
        public static readonly int[] ChangeDisplayPropertiesOrDeprecated = Tables.C8;
        public static readonly int[] C_9 = Tables.C9;
        public static readonly int[] TaggingCharacters = Tables.C9;
    }
}