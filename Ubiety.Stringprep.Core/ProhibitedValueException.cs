using System;

namespace StringPrep
{
    public class ProhibitedValueException : Exception
    {
        public ProhibitedValueException(char prohibited) : base(
            $"The string contains the prohibited value: '{prohibited}'")
        {
        }
    }
}