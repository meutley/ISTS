using System;

namespace ISTS.Domain.PostalCodes
{
    public class PostalCodeFormatException : FormatException
    {
        public PostalCodeFormatException()
            : base() { }
        
        public PostalCodeFormatException(string message)
            : base(message) { }
    }
}