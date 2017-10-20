using System;

namespace ISTS.Domain.Schedules
{
    public class OverlappingScheduleException : Exception
    {
        public OverlappingScheduleException()
            : base("The given schedule overlaps an existing schedule") { }
    }
}