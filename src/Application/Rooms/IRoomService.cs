using System;
using System.Threading.Tasks;

using ISTS.Application.Schedules;

namespace ISTS.Application.Rooms
{
    public interface IRoomService
    {
        Task<RoomSessionDto> CreateSessionAsync(RoomSessionDto session);
        Task<RoomSessionDto> RescheduleSessionAsync(Guid sessionId, DateRangeDto newSchedule);
    }
}