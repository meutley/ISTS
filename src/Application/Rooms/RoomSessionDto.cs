using System;

using ISTS.Application.Schedules;

namespace ISTS.Application.Rooms
{
    public class RoomSessionDto
    {
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }

        public DateRangeDto Schedule { get; set; }
    }
}