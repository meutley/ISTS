using System;
using System.Threading.Tasks;

using ISTS.Application.Sessions;
using ISTS.Application.Schedules;

namespace ISTS.Application.Rooms
{
    public interface IRoomService
    {
        Task<RoomDto> GetAsync(Guid id);
        
        Task<SessionDto> CreateSessionAsync(Guid roomId, SessionDto session);
        Task<SessionDto> RescheduleSessionAsync(Guid sessionId, DateRangeDto newSchedule);
        Task<SessionDto> StartSessionAsync(Guid sessionId);
        Task<SessionDto> EndSessionAsync(Guid sessionId);
    }
}