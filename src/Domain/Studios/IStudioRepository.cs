using System;
using System.Collections.Generic;

using ISTS.Domain.Schedules;

namespace ISTS.Domain.Studios
{
    public interface IStudioRepository
    {
        IEnumerable<Studio> Get();
        Studio Get(Guid id);
        StudioRoom CreateRoom(StudioRoom entity);
    }
}