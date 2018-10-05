using System.Collections.Generic;

namespace Ubiety.Stringprep.Core
{
    public class PreparationProcess : IPreparationProcess
    {
        private readonly IList<IPreparationProcess> _steps;

        internal PreparationProcess(IList<IPreparationProcess> steps)
        {
            _steps = steps;
        }

        public string Run(string input)
        {
            var result = input;
            foreach (var step in _steps) result = step.Run(result);
            return result;
        }

        public static IPreparationProcessBuilder Build()
        {
            return new PreparationProcessBuilder();
        }
    }
}