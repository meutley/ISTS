using System;

namespace ISTS.Domain.Users
{
    public interface IUserValidator
    {
        void Validate(Guid? userId, string email, string displayName, string postalCode, string password);
    }
}