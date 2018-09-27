using System;
using System.Runtime.Serialization;

namespace Ubiety.Stringprep.Core
{
    [Serializable]
    public class ProhibitedValueException : Exception
    {
        public ProhibitedValueException()
        {
        }

        public ProhibitedValueException(char prohibited)
            : base($"The string contains the prohibited value: '{prohibited}'")
        {
        }

        public ProhibitedValueException(string message)
            : base(message)
        {
        }

        public ProhibitedValueException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        protected ProhibitedValueException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}