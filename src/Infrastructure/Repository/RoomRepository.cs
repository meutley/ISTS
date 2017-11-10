using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ISTS.Domain.Rooms;
using ISTS.Domain.Common;
using ISTS.Domain.Sessions;
using ISTS.Domain.SessionRequests;
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
                .Include(r => r.RoomFunctions)
                .Include(r => r.Sessions)
                .Include(r => r.SessionRequests)
                .SingleOrDefaultAsync(r => r.Id == id);

            return room;
        }

        public async Task<RoomFunction> AddRoomFunctionAsync(Guid roomId, RoomFunction entity)
        {
            await _context.RoomFunctions.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Session> GetSessionAsync(Guid id)
        {
            var entity = await _context.Sessions
                .Include(s => s.SessionRequest)
                .SingleOrDefaultAsync(s => s.Id == id);

            return entity;
        }

        public async Task<Session> CreateSessionAsync(Guid roomId, Session entity)
        {
            var room = await _context.Rooms
                .SingleAsync(r => r.Id == roomId);
            
            if (room == null)
            {
                return null;
            }
            
            await _context.Sessions.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Session> RescheduleSessionAsync(Guid id, DateRange schedule)
        {
            var session = await _context.Sessions
                .SingleAsync(s => s.Id == id);

            session.Reschedule(schedule);

            await _context.SaveChangesAsync();

            return session;
        }

        public async Task<Session> StartSessionAsync(Guid id, DateTime time)
        {
            var session = await _context.Sessions
                .SingleAsync(s => s.Id == id);

            session.SetActualStartTime(time);

            await _context.SaveChangesAsync();

            return session;
        }

        public async Task<Session> EndSessionAsync(Guid id, DateTime time)
        {
            var session = await _context.Sessions
                .SingleAsync(s => s.Id == id);

            session.SetActualEndTime(time);

            await _context.SaveChangesAsync();

            return session;
        }

        public async Task<SessionRequest> RequestSessionAsync(SessionRequest entity)
        {
            await _context.SessionRequests.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<SessionRequest> ApproveSessionRequestAsync(SessionRequest entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<SessionRequest> RejectSessionRequestAsync(SessionRequest entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
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