using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ISTS.Domain.Schedules;

namespace ISTS.Domain.Rooms
{
    public interface IRoomRepository
    {
        Task<Room> GetAsync(Guid id);
        
        Task<RoomSession> GetSessionAsync(Guid id);
        Task<RoomSession> CreateSessionAsync(Guid roomId, RoomSession entity);
        Task<RoomSession> RescheduleSessionAsync(Guid id, DateRange schedule);
        Task<RoomSession> StartSessionAsync(Guid id, DateTime time);
        Task<RoomSession> EndSessionAsync(Guid id, DateTime time);

        Task<IEnumerable<RoomSessionSchedule>> GetScheduleAsync(Guid id, DateRange range);
    }
}