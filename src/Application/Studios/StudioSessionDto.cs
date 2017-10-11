using System;
using System.Collections.Generic;

using ISTS.Application.Schedules;

namespace ISTS.Application.Studios
{
    public class StudioSessionDto
    {
        public Guid Id { get; set; }

        public DateRangeDto ScheduledTime { get; set; }
    }
}