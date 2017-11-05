using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ISTS.Application.Common;
using ISTS.Application.Sessions;
using ISTS.Application.SessionRequests;

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

        Task<SessionRequestDto> RequestSessionAsync(SessionRequestDto model);
        Task<SessionRequestDto> ApproveSessionRequestAsync(Guid roomId, Guid requestId);
        Task<SessionRequestDto> RejectSessionRequestAsync(Guid roomId, Guid requestId, string reason);
    }
}