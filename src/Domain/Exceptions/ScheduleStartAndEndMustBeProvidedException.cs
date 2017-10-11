using System;

namespace ISTS.Domain.Exceptions
{
    public class ScheduleStartAndEndMustBeProvidedException : Exception
    {
        public ScheduleStartAndEndMustBeProvidedException()
            : base("Schedule Start and End must be provided together or not at all") { }
    }
}