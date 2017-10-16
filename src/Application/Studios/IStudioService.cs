using System;
using System.Collections.Generic;

using ISTS.Domain.Studios;

namespace ISTS.Application.Studios
{
    public interface IStudioService
    {
        StudioDto Create(StudioDto model);
        List<StudioDto> GetAll();
        StudioDto Get(Guid id);
        StudioDto Update(StudioDto model);
        
        StudioRoomDto CreateRoom(StudioRoomDto model);
    }
}