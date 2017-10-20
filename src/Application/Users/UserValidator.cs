using System;
using System.Linq;

using ISTS.Domain.Common;
using ISTS.Domain.Users;
using ISTS.Helpers.Async;

namespace ISTS.Application.Users
{
    public class UserValidator : IUserValidator
    {
        private readonly IEmailValidator _emailValidator;
        private readonly IUserRepository _userRepository;

        public UserValidator(
            IEmailValidator emailValidator,
            IUserRepository userRepository)
        {
            _emailValidator = emailValidator;
            _userRepository = userRepository;
        }

        public void Validate(Guid? userId, string email, string displayName, string postalCode, string password)
        {
            _emailValidator.Validate(email);
            
            CheckIfEmailInUse(userId, email);
        }

        private void CheckIfEmailInUse(Guid? userId, string email)
        {
            var existingEmail =
                AsyncHelper.RunSync(
                    () => _userRepository.GetAsync(u => u.Email == email && (userId == null || u.Id == userId)));

            if (existingEmail.Any())
            {
                throw new EmailInUseException(email);
            }
        }
    }
}