using System;

namespace ISTS.Domain.Exceptions
{
    public class OverlappingScheduleException : Exception
    {
        public OverlappingScheduleException()
            : base("The given schedule overlaps an existing schedule") { }
    }
}