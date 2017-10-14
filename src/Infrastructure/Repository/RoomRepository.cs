using System;
using System.Collections.Generic;
using System.Linq;

using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;

namespace ISTS.Infrastructure.Repository
{
    public class RoomRepository : IRoomRepository
    {
        public Room Get(Guid id)
        {
            return null;
        }

        public RoomSession CreateSession(Guid roomId, RoomSession entity)
        {
            return null;
        }

        public RoomSession GetSession(Guid id)
        {
            return null;
        }

        public RoomSession RescheduleSession(Guid id, DateRange schedule)
        {
            return null;
        }

        public IEnumerable<RoomSessionSchedule> GetSchedule(Guid id, DateRange range)
        {
            return Enumerable.Empty<RoomSessionSchedule>();
        }
    }
}