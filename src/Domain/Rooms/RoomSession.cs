using System;

using ISTS.Domain.Schedules;

namespace ISTS.Domain.Rooms
{
    public class RoomSession
    {
        public Guid Id { get; protected set; }

        public Guid RoomId { get; protected set; }

        public virtual Room Room { get; protected set; }

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

        public static RoomSession Create(Guid roomId, DateRange schedule)
        {
            var roomSession = new RoomSession
            {
                Id = Guid.NewGuid(),
                RoomId = roomId,
                ScheduledStartTime = schedule?.Start,
                ScheduledEndTime = schedule?.End
            };

            return roomSession;
        }

        public RoomSession Reschedule(DateRange schedule)
        {
            this.ScheduledStartTime = schedule?.Start;
            this.ScheduledEndTime = schedule?.End;
            return this;
        }

        public RoomSession SetActualStartTime(DateTime? time)
        {
            this.ActualStartTime = time;
            return this;
        }

        public RoomSession SetActualEndTime(DateTime? time)
        {
            this.ActualEndTime = time;
            return this;
        }

        public RoomSession ResetActualTime()
        {
            this.ActualStartTime = null;
            this.ActualEndTime = null;
            return this;
        }
    }
}