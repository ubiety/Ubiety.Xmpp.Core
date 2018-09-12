using System;

namespace StringPrep
{
    public class BidirectionalFormatException : Exception
    {
        public BidirectionalFormatException(string message) : base(message)
        {
        }
    }
}