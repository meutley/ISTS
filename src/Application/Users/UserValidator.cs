using System;

using ISTS.Domain.Common;
using ISTS.Domain.Users;

namespace ISTS.Application.Users
{
    public class UserValidator : IUserValidator
    {
        private readonly IEmailValidator _emailValidator;

        public UserValidator(
            IEmailValidator emailValidator)
        {
            _emailValidator = emailValidator;
        }

        public void Validate(string email, string displayName, string postalCode, string password)
        {
            _emailValidator.Validate(email);
        }
    }
}