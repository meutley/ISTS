using System;

using ISTS.Domain.Schedules;

namespace ISTS.Domain.Rooms
{
    public class RoomSession
    {
        public Guid Id { get; protected set; }

        public Guid RoomId { get; protected set; }

        public DateRange Schedule { get; protected set; }

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

        public static RoomSession Reschedule(RoomSession session, DateRange schedule)
        {
            var roomSession = new RoomSession
            {
                Id = session.Id,
                RoomId = session.RoomId,
                Schedule = schedule
            };

            return roomSession;
        }
    }
}