using System;

namespace ISTS.Domain.Exceptions
{
    public class SessionAlreadyEndedException : Exception
    {
        public SessionAlreadyEndedException()
            : base("The session has already been ended") { }
    }
}