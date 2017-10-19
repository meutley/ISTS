using System;

using ISTS.Application.Rooms;
using ISTS.Application.Schedules;

namespace ISTS.Application.Sessions
{
    public class SessionDto
    {
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }

        public DateRangeDto Schedule { get; set; }

        public DateTime? ActualStartTime { get; set; }

        public DateTime? ActualEndTime { get; set; }
    }
}