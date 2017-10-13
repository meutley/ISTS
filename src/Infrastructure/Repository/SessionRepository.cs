using System;
using System.Collections.Generic;
using System.Linq;

using ISTS.Domain.Schedules;
using ISTS.Domain.Sessions;

namespace ISTS.Infrastructure.Repository
{
    public class SessionRepository : ISessionRepository
    {
        public IEnumerable<Session> Get()
        {
            return Enumerable.Empty<Session>();
        }

        public Session Get(Guid id)
        {
            return null;
        }

        public Session SetSchedule(Guid sessionId, DateRange schedule)
        {
            return null;
        }
    }
}