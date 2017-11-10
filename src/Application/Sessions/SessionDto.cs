using System;

using ISTS.Application.Common;
using ISTS.Application.Rooms;

namespace ISTS.Application.Sessions
{
    public class SessionDto
    {
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }

        public Guid? RoomFunctionId { get; set; }

        public DateRangeDto Schedule { get; set; }

        public DateTime? ActualStartTime { get; set; }

        public DateTime? ActualEndTime { get; set; }

        public Guid? SessionRequestId { get; set; }
    }
}