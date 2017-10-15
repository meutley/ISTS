using System;
using System.Collections.Generic;

using ISTS.Domain.Studios;

namespace ISTS.Application.Studios
{
    public interface IStudioService
    {
        StudioDto Create(string name, string friendlyUrl);
        List<StudioDto> GetAll();
        StudioDto Get(Guid id);
        
        StudioRoomDto CreateRoom(Guid studioId, string name);
    }
}