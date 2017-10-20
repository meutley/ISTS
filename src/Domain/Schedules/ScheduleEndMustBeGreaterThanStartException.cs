using System;

namespace ISTS.Domain.Schedules
{
    public class ScheduleEndMustBeGreaterThanStartException : Exception
    {
        public ScheduleEndMustBeGreaterThanStartException()
            : base("Schedule End must be greater than Start") { }
    }
}