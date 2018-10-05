using System.Text;

namespace Ubiety.Stringprep.Core
{
    public interface IPreparationProcessBuilder
    {
        IPreparationProcessBuilder WithNormalizationStep();
        IPreparationProcessBuilder WithNormalizationStep(NormalizationForm form);
        IPreparationProcessBuilder WithMappingStep(IMappingTable mappingTable);
        IPreparationProcessBuilder WithProhibitedValueStep(IValueRangeTable prohibitedTable);
        IPreparationProcessBuilder WithBidirectionalStep();

        IPreparationProcessBuilder WithBidirectionalStep(IValueRangeTable prohibited, IValueRangeTable ral,
            IValueRangeTable l);

        IPreparationProcess Compile();
    }
}