using System;

using ISTS.Application.Schedules;

namespace ISTS.Application.Sessions
{
    public interface ISessionService
    {
        SessionDto Reschedule(Guid sessionId, DateRangeDto schedule);
    }
}