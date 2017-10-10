using System;

namespace ISTS.Application.Sessions
{
    public interface ISessionService
    {
        void Reschedule(Guid sessionId, DateTime startDateTime, DateTime endDateTime);
    }
}