using System;

using ISTS.Domain.Common;

namespace ISTS.Domain.Rooms
{
    public class RoomSessionSchedule
    {
        public Guid SessionId { get; protected set; }

        public DateRange Schedule { get; protected set; }

        public static RoomSessionSchedule Create(Guid sessionId, DateRange schedule)
        {
            var result = new RoomSessionSchedule
            {
                SessionId = sessionId,
                Schedule = schedule
            };

            return result;
        }
    }
}