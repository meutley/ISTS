using System;

namespace ISTS.Domain.Sessions
{
    public class SessionAlreadyEndedException : Exception
    {
        public SessionAlreadyEndedException()
            : base("The session has already been ended") { }
    }
}