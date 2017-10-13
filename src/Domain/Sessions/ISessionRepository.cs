using System;
using System.Collections.Generic;

using ISTS.Domain.Schedules;

namespace ISTS.Domain.Sessions
{
    public interface ISessionRepository
    {
        IEnumerable<Session> Get();
        Session Get(Guid id);
        Session SetSchedule(Guid sessionId, DateRange schedule);
    }
}