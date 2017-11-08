using System;
using System.Collections.Generic;

using ISTS.Domain.Studios;
using ISTS.Helpers.Async;
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

        public int TimeZoneId { get; protected set; }

        public UserTimeZone TimeZone { get; protected set; }

        public static User Create(
            IUserValidator userValidator,
            IUserPasswordService userPasswordService,
            string email,
            string displayName,
            string plainPassword,
            string postalCode,
            int timeZoneId)
        {
            AsyncHelper.RunSync(() => userValidator.ValidateAsync(null, email, displayName, plainPassword, postalCode));
            
            byte[] passwordHash;
            byte[] passwordSalt;
            userPasswordService.CreateHash(plainPassword, out passwordHash, out passwordSalt);
            
            var user = new User
            {
                Id = Guid.NewGuid(),
                Email = email,
                DisplayName = displayName,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                PostalCode = postalCode,
                TimeZoneId = timeZoneId
            };

            return user;
        }
    }
}