using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StringPrep
{
  internal class NormalizationStep : IPreparationProcess
  {
    private readonly NormalizationForm _normalizationForm;

    public NormalizationStep(NormalizationForm normalizationForm)
    {
      _normalizationForm = normalizationForm;
    }

    public string Run(string input)
    {
      return input.Normalize(_normalizationForm);
    }
  }
}
