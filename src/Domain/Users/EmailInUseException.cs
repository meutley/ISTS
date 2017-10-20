using System;

namespace ISTS.Domain.Users
{
    public class EmailInUseException : Exception
    {
        public EmailInUseException(string email)
            : base(string.Format("The email address {0} is already in use", email))
        { }
    }
}