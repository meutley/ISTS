using System;

using ISTS.Domain;

namespace ISTS.Application.Sessions
{
    public interface ISessionRepository
    {
        Session Get(Guid sessionId);
    }
}