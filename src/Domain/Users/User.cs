using System;
using System.Collections.Generic;

using ISTS.Domain.Studios;
using ISTS.Helpers.Validation;

namespace ISTS.Domain.Users
{
    public class User : IAggregateRoot
    {
        public Guid Id { get; protected set; }

        public string Email { get; protected set; }

        public string DisplayName { get; protected set; }

        public byte[] PasswordHash { get; protected set; }

        public byte[] PasswordSalt { get; protected set; }

        public virtual ICollection<Studio> Studios { get; set; }

        public string PostalCode { get; protected set; }

        public static User Create(
            IUserValidator userValidator,
            string email,
            string displayName,
            string plainPassword,
            string postalCode)
        {
            userValidator.Validate(null, email, displayName, plainPassword, postalCode);
            
            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHash(plainPassword, out passwordHash, out passwordSalt);
            
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                DisplayName = displayName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PostalCode = postalCode
            };

            return user;
        }

        public bool ValidatePasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
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

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
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