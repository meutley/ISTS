using System;
using System.Collections.Generic;

namespace ISTS.Domain.Sessions
{
    public interface ISessionRepository
    {
        IEnumerable<Session> Get();
    }
}