using System;

namespace ISTS.Domain.Studios
{
    public class StudioUrlInUseException : Exception
    {
        public StudioUrlInUseException(string message)
            : base(message) { }
    }
}