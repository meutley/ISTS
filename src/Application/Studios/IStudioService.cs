using System;

using ISTS.Domain.Studios;

namespace ISTS.Application.Studios
{
    public interface IStudioService
    {
        StudioRoomDto CreateRoom(Guid studioId, StudioRoomDto room);
    }
}