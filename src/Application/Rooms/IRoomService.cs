using System;

using ISTS.Domain.Rooms;

namespace ISTS.Application.Rooms
{
    public interface IRoomService
    {
        RoomSessionDto CreateSession(RoomSessionDto session);
    }
}