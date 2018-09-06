using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringPrep
{
  internal class PreparationProcessBuilder : IPreparationProcessBuilder
  {
    private readonly IList<IPreparationProcess> _steps = new List<IPreparationProcess>();

    public IPreparationProcessBuilder WithBidirectionalStep()
    {
      return WithBidirectionalStep(
        ValueRangeTable.Create(Prohibited.ChangeDisplayPropertiesOrDeprecated),
        ValueRangeTable.Create(Bidirectional.R_AL),
        ValueRangeTable.Create(Bidirectional.L));
    }

    public IPreparationProcessBuilder WithBidirectionalStep(IValueRangeTable prohibited, IValueRangeTable ral, IValueRangeTable l)
    {
      var step = new BidirectionalStep(prohibited, ral, l);
      _steps.Add(step);
      return this;
    }

    public IPreparationProcessBuilder WithMappingStep(IMappingTable mappingTable)
    {
      var step = new MappingStep(mappingTable);
      _steps.Add(step);
      return this;
    }

    public IPreparationProcessBuilder WithNormalizationStep()
    {
      return WithNormalizationStep(NormalizationForm.FormKC);
    }

    public IPreparationProcessBuilder WithNormalizationStep(NormalizationForm form)
    {
      var step = new NormalizationStep(form);
      _steps.Add(step);
      return this;
    }

    public IPreparationProcessBuilder WithProhibitedValueStep(IValueRangeTable prohibitedTable)
    {
      var step = new ProhibitedValueStep(prohibitedTable);
      _steps.Add(step);
      return this;
    }

    public IPreparationProcess Compile()
    {
      return new PreparationProcess(_steps);
    }
  }
}
