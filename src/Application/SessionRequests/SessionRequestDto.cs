using System;

using ISTS.Application.Common;

namespace ISTS.Application.SessionRequests
{
    public class SessionRequestDto
    {
        public Guid Id { get; set; }

        public Guid RoomId { get; set; }

        public Guid RequestingUserId { get; set; }

        public DateRangeDto RequestedTime { get; set; }

        public int SessionRequestStatusId { get; set; }

        public string RejectedReason { get; set; }
    }
}