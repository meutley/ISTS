using System;
using System.Collections.Generic;

using ISTS.Domain.Schedules;

namespace ISTS.Domain.Rooms
{
    public interface IRoomRepository
    {
        Room Get(Guid id);
        
        RoomSession GetSession(Guid id);
        RoomSession CreateSession(Guid roomId, RoomSession entity);
        RoomSession RescheduleSession(Guid id, DateRange schedule);
        RoomSession StartSession(Guid id, DateTime time);
        RoomSession EndSession(Guid id, DateTime time);

        IEnumerable<RoomSessionSchedule> GetSchedule(Guid id, DateRange range);
    }
}