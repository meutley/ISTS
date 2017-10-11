using System;

using ISTS.Domain.Schedules;

namespace ISTS.Domain.Studios
{
    public class StudioSessionSchedule
    {
        public Guid SessionId { get; protected set; }

        public DateRange Schedule { get; protected set; }

        public static StudioSessionSchedule Create(Guid sessionId, DateRange schedule)
        {
            var result = new StudioSessionSchedule
            {
                SessionId = sessionId,
                Schedule = schedule
            };

            return result;
        }
    }
}