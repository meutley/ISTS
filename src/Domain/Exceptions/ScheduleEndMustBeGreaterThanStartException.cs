using System;

namespace ISTS.Domain.Exceptions
{
    public class ScheduleEndMustBeGreaterThanStartException : Exception
    {
        public ScheduleEndMustBeGreaterThanStartException()
            : base("Schedule End must be greater than Start") { }
    }
}