using System;
using System.Collections.Generic;
using System.Linq;

using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;
using ISTS.Infrastructure.Model;

namespace ISTS.Infrastructure.Repository
{
    public class RoomRepository : IRoomRepository
    {
        private readonly IstsContext _context;

        public RoomRepository(
            IstsContext context)
        {
            _context = context;
        }
        
        public Room Get(Guid id)
        {
            return null;
        }

        public RoomSession GetSession(Guid id)
        {
            return null;
        }

        public RoomSession CreateSession(Guid roomId, RoomSession entity)
        {
            return null;
        }

        public RoomSession RescheduleSession(Guid id, DateRange schedule)
        {
            return null;
        }

        public RoomSession StartSession(Guid id, DateTime time)
        {
            return null;
        }

        public RoomSession EndSession(Guid id, DateTime time)
        {
            return null;
        }

        public IEnumerable<RoomSessionSchedule> GetSchedule(Guid id, DateRange range)
        {
            var sessions = _context.Sessions
                .Where(s => s.RoomId == id)
                .ToList();

            var schedule = sessions
                .Select(s => RoomSessionSchedule.Create(s.Id, s.Schedule));

            return schedule;
        }
    }
}