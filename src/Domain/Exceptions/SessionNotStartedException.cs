using System;

namespace ISTS.Domain.Exceptions
{
    public class SessionNotStartedException : Exception
    {
        public SessionNotStartedException()
            : base("A session cannot be ended before it has been started") { }
        
        public SessionNotStartedException(string message)
            : base(message) { }
    }
}