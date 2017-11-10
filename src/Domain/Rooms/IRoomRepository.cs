using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ISTS.Domain.Common;
using ISTS.Domain.Sessions;
using ISTS.Domain.SessionRequests;

namespace ISTS.Domain.Rooms
{
    public interface IRoomRepository
    {
        Task<Room> GetAsync(Guid id);
        
        Task<RoomFunction> AddRoomFunctionAsync(Guid roomId, RoomFunction entity);
        
        Task<Session> GetSessionAsync(Guid id);
        Task<Session> CreateSessionAsync(Guid roomId, Session entity);
        Task<Session> RescheduleSessionAsync(Guid id, DateRange schedule);
        Task<Session> StartSessionAsync(Guid id, DateTime time);
        Task<Session> EndSessionAsync(Guid id, DateTime time);

        Task<SessionRequest> RequestSessionAsync(SessionRequest entity);
        Task<SessionRequest> ApproveSessionRequestAsync(SessionRequest entity);
        Task<SessionRequest> RejectSessionRequestAsync(SessionRequest entity);

        Task<IEnumerable<RoomSessionSchedule>> GetScheduleAsync(Guid id, DateRange range);
    }
}