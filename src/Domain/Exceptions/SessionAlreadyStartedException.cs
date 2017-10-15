using System;

namespace ISTS.Domain.Exceptions
{
    public class SessionAlreadyStartedException : Exception
    {
        public SessionAlreadyStartedException()
            : base("The session has already been started") { }
    }
}