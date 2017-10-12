using System;
using System.Collections.Generic;
using System.Linq;

using ISTS.Domain.Studios;

namespace ISTS.Infrastructure.Repository
{
    public class StudioRepository : IStudioRepository
    {
        public IEnumerable<Studio> Get()
        {
            return Enumerable.Empty<Studio>();
        }

        public Studio Get(Guid id)
        {
            return Studio.Create("asdf", "asdf");
        }

        public IEnumerable<StudioSessionSchedule> GetSchedule(Guid studioId, DateTime from, DateTime to)
        {
            return Enumerable.Empty<StudioSessionSchedule>();
        }
    }
}