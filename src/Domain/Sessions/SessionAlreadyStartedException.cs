using System;

namespace ISTS.Domain.Sessions
{
    public class SessionAlreadyStartedException : Exception
    {
        public SessionAlreadyStartedException()
            : base("The session has already been started") { }
    }
}