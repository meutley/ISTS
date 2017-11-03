using System;

using ISTS.Application.Common;
using ISTS.Application.Rooms;

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