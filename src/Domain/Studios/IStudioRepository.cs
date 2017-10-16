using System;
using System.Collections.Generic;

using ISTS.Domain.Schedules;

namespace ISTS.Domain.Studios
{
    public interface IStudioRepository
    {
        Studio Create(string name, string friendlyUrl);
        IEnumerable<Studio> Get();
        Studio Get(Guid id);
        
        StudioRoom CreateRoom(Guid studioId, string name);
    }
}