using System;

using ISTS.Domain.Common;
using ISTS.Domain.Rooms;
using ISTS.Domain.Users;
using ISTS.Helpers.Async;

namespace ISTS.Domain.Sessions
{
    public class SessionRequest : IAggregateRoot
    {
        public Guid Id { get; protected set; }

        public Guid RequestingUserId { get; protected set; }

        public virtual User RequestingUser { get; protected set; }

        public Guid RoomId { get; protected set; }

        public virtual Room Room { get; protected set; }

        public DateTime RequestedStartTime { get; protected set; }

        public DateTime RequestedEndTime { get; protected set; }

        public DateRange RequestedTime
        {
            get
            {
                return DateRange.Create(RequestedStartTime, RequestedEndTime);
            }
        }

        public static SessionRequest Create(
            Guid requestingUserId,
            Guid roomId,
            DateTime requestedStartTime,
            DateTime requestedEndTime)
        {
            var request = new SessionRequest
            {
                RequestingUserId = requestingUserId,
                RoomId = roomId,
                RequestedStartTime = requestedStartTime,
                RequestedEndTime = requestedEndTime
            };

            return request;
        }
    }
}