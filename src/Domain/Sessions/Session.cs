using System;

using ISTS.Domain.Common;
using ISTS.Domain.Rooms;
using ISTS.Domain.SessionRequests;

namespace ISTS.Domain.Sessions
{
    public class Session
    {
        public Guid Id { get; protected set; }

        public Guid RoomId { get; protected set; }
        
        public DateTime? ScheduledStartTime { get; protected set; }

        public DateTime? ScheduledEndTime { get; protected set; }

        public DateRange Schedule
        {
            get
            {
                return
                    !ScheduledStartTime.HasValue
                    ? null
                    : DateRange.Create(ScheduledStartTime.Value, ScheduledEndTime.Value);
            }
        }

        public DateTime? ActualStartTime { get; protected set; }

        public DateTime? ActualEndTime { get; protected set; }

        public Guid? SessionRequestId { get; protected set; }

        public virtual SessionRequest SessionRequest { get; protected set; }

        public static Session Create(Guid roomId, DateRange schedule, Guid? sessionRequestId = null)
        {
            var roomSession = new Session
            {
                Id = Guid.NewGuid(),
                RoomId = roomId,
                ScheduledStartTime = schedule?.Start,
                ScheduledEndTime = schedule?.End,
                SessionRequestId = sessionRequestId
            };

            return roomSession;
        }

        public Session Reschedule(DateRange schedule)
        {
            this.ScheduledStartTime = schedule?.Start;
            this.ScheduledEndTime = schedule?.End;
            return this;
        }

        public Session SetActualStartTime(DateTime? time)
        {
            this.ActualStartTime = time;
            return this;
        }

        public Session SetActualEndTime(DateTime? time)
        {
            this.ActualEndTime = time;
            return this;
        }

        public Session ResetActualTime()
        {
            this.ActualStartTime = null;
            this.ActualEndTime = null;
            return this;
        }
    }
}