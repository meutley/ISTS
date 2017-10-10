using System;

namespace ISTS.Application.Exceptions
{
    public class InvalidDateRangeException : Exception
    {
        public InvalidDateRangeException(string message)
            : base(message) { }
    }
}