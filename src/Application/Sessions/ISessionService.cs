using System;

namespace ISTS.Application.Sessions
{
    public interface ISessionService
    {
        SessionDto Create(SessionDto session);
        SessionDto Get(Guid id);
    }
}