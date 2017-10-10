using System;

namespace ISTS.Domain.Exceptions
{
    public class InvalidDateRangeException : Exception
    {
        public InvalidDateRangeException(string message)
            : base(message) { }
    }
}