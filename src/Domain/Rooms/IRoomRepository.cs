using System;
using System.Collections.Generic;

using ISTS.Domain.Schedules;

namespace ISTS.Domain.Rooms
{
    public interface IRoomRepository
    {
        Room Get(Guid id);
        RoomSession CreateSession(Guid roomId, RoomSession entity);
        IEnumerable<RoomSessionSchedule> GetSchedule(Guid id, DateRange range);
    }
}