using System;

namespace ISTS.Domain.Schedules
{
    public class InvalidDateRangeException : Exception
    {
        public InvalidDateRangeException(string message)
            : base(message) { }
    }
}