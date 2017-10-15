using System;

using ISTS.Domain.Studios;

namespace ISTS.Application.Studios
{
    public interface IStudioService
    {
        StudioDto Create(string name, string friendlyUrl);
        StudioRoomDto CreateRoom(Guid studioId, string name);
    }
}