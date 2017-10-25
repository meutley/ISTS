using System;

using ISTS.Domain.Users;
using ISTS.Helpers.Validation;

namespace ISTS.Application.Users
{
    public class UserPasswordService : IUserPasswordService
    {
        public bool ValidateHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            ArgumentNotNullValidator.Validate(password, nameof(password));

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace", nameof(password));
            }

            if (passwordHash.Length != 64)
            {
                throw new ArgumentException("Expected 64-byte password hash", nameof(passwordHash));
            }

            if (passwordSalt.Length != 128)
            {
                throw new ArgumentException("Expected 128-byte password salt", nameof(passwordSalt));
            }

            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for (var x = 0; x < computedHash.Length; x++)
                {
                    if (computedHash[x] != passwordHash[x])
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        public void CreateHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException(nameof(password));
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace", nameof(password));
            }
 
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}