using System;
using System.Collections.Generic;

using ISTS.Domain.Studios;

namespace ISTS.Domain.Users
{
    public class User : IAggregateRoot
    {
        public Guid Id { get; protected set; }

        public string Email { get; protected set; }

        public string DisplayName { get; protected set; }

        public string PostalCode { get; protected set; }

        public virtual ICollection<Studio> Studios { get; set; }

        public byte[] PasswordHash { get; protected set; }

        public byte[] PasswordSalt { get; protected set; }

        public static User Create(
            string email,
            string displayName,
            string postalCode,
            string plainPassword)
        {
            byte[] passwordHash;
            byte[] passwordSalt;
            CreatePasswordHash(plainPassword, out passwordHash, out passwordSalt);
            
            var user = new User
            {
                Email = email,
                DisplayName = displayName,
                PostalCode = postalCode,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            return user;
        }

        private static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            if (password == null)
            {
                throw new ArgumentNullException("password");
            }

            if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentException("Value cannot be empty or whitespace only string.", "password");
            }
 
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}