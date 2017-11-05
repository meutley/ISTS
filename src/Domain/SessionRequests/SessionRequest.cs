using System;

using ISTS.Domain.Common;
using ISTS.Domain.Rooms;
using ISTS.Domain.Users;
using ISTS.Helpers.Async;

namespace ISTS.Domain.SessionRequests
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

        public int SessionRequestStatusId { get; protected set; }

        public virtual SessionRequestStatus SessionRequestStatus { get; protected set; }

        public string RejectedReason { get; protected set; }

        public static SessionRequest Create(
            Guid requestingUserId,
            Guid roomId,
            DateTime requestedStartTime,
            DateTime requestedEndTime)
        {
            var request = new SessionRequest
            {
                Id = Guid.NewGuid(),
                RequestingUserId = requestingUserId,
                RoomId = roomId,
                RequestedStartTime = requestedStartTime,
                RequestedEndTime = requestedEndTime,
                SessionRequestStatusId = (int)SessionRequests.SessionRequestStatusId.Pending
            };

            return request;
        }

        public SessionRequest Approve()
        {
            ValidateRequestIsPending(this.SessionRequestStatusId);
            
            this.SessionRequestStatusId = (int)SessionRequests.SessionRequestStatusId.Approved;
            this.RejectedReason = null;
            return this;
        }

        public SessionRequest Reject(string reason)
        {
            ValidateRequestIsPending(this.SessionRequestStatusId);
            
            this.SessionRequestStatusId = (int)SessionRequests.SessionRequestStatusId.Rejected;
            this.RejectedReason = reason != null ? reason.Trim() : null;
            return this;
        }

        private static void ValidateRequestIsPending(int statusId)
        {
            if (statusId != (int)SessionRequests.SessionRequestStatusId.Pending)
            {
                var type =
                    statusId == (int)SessionRequests.SessionRequestStatusId.Approved
                    ? "approved"
                    : "rejected";

                throw new DomainValidationException(new InvalidOperationException($"The session request has already been {type}"));
            }
        }
    }
}