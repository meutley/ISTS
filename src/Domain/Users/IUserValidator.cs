using System;

namespace ISTS.Domain.Users
{
    public interface IUserValidator
    {
        void Validate(string email, string displayName, string postalCode, string password);
    }
}