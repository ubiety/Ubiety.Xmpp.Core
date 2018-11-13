using System.Text;

namespace Ubiety.Stringprep.Core
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