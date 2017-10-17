using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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
        
        public async Task<Room> GetAsync(Guid id)
        {
            return null;
        }

        public async Task<RoomSession> GetSessionAsync(Guid id)
        {
            return null;
        }

        public async Task<RoomSession> CreateSessionAsync(Guid roomId, RoomSession entity)
        {
            return null;
        }

        public async Task<RoomSession> RescheduleSessionAsync(Guid id, DateRange schedule)
        {
            return null;
        }

        public async Task<RoomSession> StartSessionAsync(Guid id, DateTime time)
        {
            return null;
        }

        public async Task<RoomSession> EndSessionAsync(Guid id, DateTime time)
        {
            return null;
        }

        public async Task<IEnumerable<RoomSessionSchedule>> GetScheduleAsync(Guid id, DateRange range)
        {
            var sessions = await _context.Sessions
                .Where(s => s.RoomId == id)
                .ToListAsync();

            var schedule = sessions.Select(s => RoomSessionSchedule.Create(s.Id, s.Schedule));

            return schedule;
        }
    }
}