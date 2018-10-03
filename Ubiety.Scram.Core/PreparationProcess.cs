using Ubiety.Stringprep.Core;

namespace Ubiety.Scram.Core
{
  internal static class SaslPrep
  {
    private static IPreparationProcess Create()
    {
      return PreparationProcess.Build()
        .WithMappingStep(MappingTable.Build()
          .WithValueRangeTable(Prohibited.ASCIISpaceCharacters, ' ')
          .WithMappingTable(Mapping.MappedToNothing)
          .Compile())
        .WithNormalizationStep()
        .WithProhibitedValueStep(ValueRangeTable.Create(
          Prohibited.NonASCIISpaceCharacters,
          Prohibited.ASCIIControlCharacters,
          Prohibited.NonASCIIControlCharacters,
          Prohibited.PrivateUseCharacters,
          Prohibited.NonCharacterCodePoints,
          Prohibited.SurrogateCodePoints,
          Prohibited.InappropriateForPlainText,
          Prohibited.InappropriateForCanonicalRepresentation,
          Prohibited.TaggingCharacters))
        .WithBidirectionalStep()
        .WithProhibitedValueStep(ValueRangeTable.Create(
          Unassigned.UnassignedCodePoints))
        .Compile();
    }
    
    public static string Run(string input)
    {
      return Create().Run(input);
    }
  }
}
