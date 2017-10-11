using System;
using System.Collections.Generic;

namespace ISTS.Domain.Studios
{
    public interface IStudioRepository
    {
        IEnumerable<Studio> Get();
        Studio Get(Guid id);
        IEnumerable<StudioSessionSchedule> GetSchedule(Guid studioId, DateTime from, DateTime to);
    }
}