using System;

using ISTS.Domain.Schedules;

namespace ISTS.Domain.Rooms
{
    public class RoomSession
    {
        public Guid Id { get; protected set; }

        public Guid RoomId { get; protected set; }

        public DateRange Schedule { get; protected set; }

        public DateTime? ActualStartTime { get; protected set; }

        public DateTime? ActualEndTime { get; protected set; }

        public static RoomSession Create(Guid roomId, DateRange schedule)
        {
            var roomSession = new RoomSession
            {
                Id = Guid.NewGuid(),
                RoomId = roomId,
                Schedule = schedule
            };

            return roomSession;
        }

        public RoomSession Reschedule(DateRange schedule)
        {
            this.Schedule = schedule;
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
    }
}