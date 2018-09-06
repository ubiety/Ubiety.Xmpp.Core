using System.Collections.Generic;

namespace StringPrep
{
  public class PreparationProcess : IPreparationProcess
  {
    public static IPreparationProcessBuilder Build()
    {
      return new PreparationProcessBuilder();
    }

    private readonly IList<IPreparationProcess> _steps;

    internal PreparationProcess(IList<IPreparationProcess> steps)
    {
      _steps = steps;
    }

    public string Run(string input)
    {
      string result = input;
      foreach (var step in _steps)
      {
        result = step.Run(result);
      }
      return result;
    }
  }
}
