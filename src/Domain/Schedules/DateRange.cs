using System;

namespace ISTS.Domain.Schedules
{
    public class DateRange
    {
        public DateTime Start { get; protected set; }

        public DateTime End { get; protected set; }

        public static DateRange Create(DateTime start, DateTime end)
        {
            return new DateRange
            {
                Start = start,
                End = end
            };
        }
    }
}