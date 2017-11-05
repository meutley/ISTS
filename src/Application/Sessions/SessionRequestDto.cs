using System;

using ISTS.Application.Common;

namespace ISTS.Application.Sessions
{
    public class SessionRequestDto
    {
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }

        public Guid RequestingUserId { get; set; }

        public DateRangeDto RequestedTime { get; set; }
    }
}