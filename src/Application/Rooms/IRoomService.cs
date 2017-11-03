using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ISTS.Application.Common;
using ISTS.Application.Sessions;

namespace ISTS.Application.Rooms
{
    public interface IRoomService
    {
        Task<RoomDto> GetAsync(Guid id);
        
        Task<List<SessionDto>> GetSessions(Guid roomId);
        Task<SessionDto> CreateSessionAsync(Guid roomId, SessionDto session);
        Task<SessionDto> RescheduleSessionAsync(Guid roomId, Guid sessionId, DateRangeDto newSchedule);
        Task<SessionDto> StartSessionAsync(Guid roomId, Guid sessionId);
        Task<SessionDto> EndSessionAsync(Guid roomId, Guid sessionId);
    }
}