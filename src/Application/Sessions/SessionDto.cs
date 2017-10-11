using System;

using ISTS.Application.Schedules;

namespace ISTS.Application.Sessions
{
    public class SessionDto
    {
        public Guid Id { get; set; }

        public Guid StudioId { get; set; }

        public DateRangeDto ScheduledTime { get; set; }
    }
}