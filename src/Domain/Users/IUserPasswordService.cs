using System;

namespace ISTS.Domain.Users
{
    public interface IUserPasswordService
    {
        void CreateHash(string password, out byte[] passwordHash, out byte[] passwordSalt);
        bool ValidateHash(string password, byte[] passwordHash, byte[] passwordSalt);
    }
}