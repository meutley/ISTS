using System;

using ISTS.Domain;

namespace ISTS.Application.Sessions
{
    public interface ISessionScheduleEntryRepository
    {
        SessionScheduleEntry GetCurrentSchedule(Guid sessionId);
        IEnumerable<SessionScheduleEntry> GetAllSchedules(Guid sessionId);
    }
}