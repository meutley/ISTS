using System;

namespace ISTS.Domain.Sessions
{
    public class SessionNotEndedException : Exception
    {
        public SessionNotEndedException()
            : base("The session has not ended") { }
        
        public SessionNotEndedException(string message)
            : base(message) { }
    }
}