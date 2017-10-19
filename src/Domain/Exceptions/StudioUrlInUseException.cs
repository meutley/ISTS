using System;

namespace ISTS.Domain.Exceptions
{
    public class StudioUrlInUseException : Exception
    {
        public StudioUrlInUseException(string message)
            : base(message) { }
    }
}