using System;

using ISTS.Application.Schedules;

namespace ISTS.Application.Rooms
{
    public interface IRoomService
    {
        RoomSessionDto CreateSession(RoomSessionDto session);
        RoomSessionDto RescheduleSession(Guid sessionId, DateRangeDto newSchedule);
    }
}