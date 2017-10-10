using System;
using System.Collections.Generic;

using ISTS.Domain.Sessions;

namespace ISTS.Application.Sessions
{
    public interface ISessionScheduleEntryRepository
    {
        SessionScheduleEntry GetCurrentSchedule(Guid sessionId);
        IEnumerable<SessionScheduleEntry> GetAllSchedules(Guid sessionId);
    }
}