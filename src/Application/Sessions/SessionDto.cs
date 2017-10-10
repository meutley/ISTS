using System;

using ISTS.Application.Schedules;
using ISTS.Application.Studios;

namespace ISTS.Application.Sessions
{
    public class SessionDto
    {
        public Guid Id { get; set; }

        public StudioDto Studio { get; set; }

        public DateRangeDto ScheduledTime { get; set; }
    }
}