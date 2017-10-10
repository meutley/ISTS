using System;

using ISTS.Domain.Sessions;

namespace ISTS.Application.Sessions
{
    public interface ISessionRepository
    {
        Session Get(Guid sessionId);
    }
}