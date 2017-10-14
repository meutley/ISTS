using System;
using System.Collections.Generic;

using ISTS.Domain.Schedules;

namespace ISTS.Domain.Rooms
{
    public interface IRoomRepository
    {
        Room Get(Guid id);
        RoomSession CreateSession(Guid roomId, RoomSession entity);
        RoomSession GetSession(Guid id);
        RoomSession RescheduleSession(Guid id, DateRange schedule);
        IEnumerable<RoomSessionSchedule> GetSchedule(Guid id, DateRange range);
    }
}