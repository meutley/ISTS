using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;
using ISTS.Domain.Sessions;
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
            var room = await _context.Rooms
                .Include(r => r.Sessions)
                .SingleOrDefaultAsync(r => r.Id == id);

            return room;
        }

        public async Task<Session> GetSessionAsync(Guid id)
        {
            return null;
        }

        public async Task<Session> CreateSessionAsync(Guid roomId, Session entity)
        {
            return null;
        }

        public async Task<Session> RescheduleSessionAsync(Guid id, DateRange schedule)
        {
            return null;
        }

        public async Task<Session> StartSessionAsync(Guid id, DateTime time)
        {
            return null;
        }

        public async Task<Session> EndSessionAsync(Guid id, DateTime time)
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