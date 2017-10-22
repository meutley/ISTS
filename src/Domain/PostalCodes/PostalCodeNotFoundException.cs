using System;

namespace ISTS.Domain.PostalCodes
{
    public class PostalCodeNotFoundException : Exception
    {
        public PostalCodeNotFoundException()
            : base("The postal code was not found")
        { }
    }
}