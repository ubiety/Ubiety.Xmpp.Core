using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringPrep
{
  public interface IPreparationProcessBuilder
  {
    IPreparationProcessBuilder WithNormalizationStep();
    IPreparationProcessBuilder WithNormalizationStep(NormalizationForm form);
    IPreparationProcessBuilder WithMappingStep(IMappingTable mappingTable);
    IPreparationProcessBuilder WithProhibitedValueStep(IValueRangeTable prohibitedTable);
    IPreparationProcessBuilder WithBidirectionalStep();
    IPreparationProcessBuilder WithBidirectionalStep(IValueRangeTable prohibited, IValueRangeTable ral, IValueRangeTable l);
    IPreparationProcess Compile();
  }
}
