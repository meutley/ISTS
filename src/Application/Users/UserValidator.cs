using System;
using System.Linq;
using System.Text.RegularExpressions;

using ISTS.Domain.Common;
using ISTS.Domain.PostalCodes;
using ISTS.Domain.Users;
using ISTS.Helpers.Async;
using ISTS.Helpers.Validation;

namespace ISTS.Application.Users
{
    public class UserValidator : IUserValidator
    {
        private readonly IEmailValidator _emailValidator;
        private readonly IUserRepository _userRepository;
        private readonly IPostalCodeValidator _postalCodeValidator;

        private static readonly string DisplayNameRegexPattern = @"^[^\s][\s\S]+$";
        private static readonly int DisplayNameMinLength = 5;
        private static readonly int DisplayNameMaxLength = 50;

        public UserValidator(
            IEmailValidator emailValidator,
            IPostalCodeValidator postalCodeValidator,
            IUserRepository userRepository)
        {
            _emailValidator = emailValidator;
            _postalCodeValidator = postalCodeValidator;
            _userRepository = userRepository;
        }

        public void Validate(Guid? userId, string email, string displayName, string password, string postalCode)
        {
            ArgumentNotNullValidator.Validate(email, nameof(email));
            ArgumentNotNullValidator.Validate(displayName, nameof(displayName));
            ArgumentNotNullValidator.Validate(postalCode, nameof(postalCode));
            
            _emailValidator.Validate(email);
            
            AsyncHelper.RunSync(() =>
                _postalCodeValidator.ValidateAsync(
                    postalCode,
                    PostalCodeValidatorTypes.Format | PostalCodeValidatorTypes.Exists));
            
            CheckIfEmailInUse(userId, email);
            ValidateDisplayName(displayName);
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

        private void ValidateDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException("Display Name must not be empty or whitespace", nameof(displayName));
            }

            var doesPatternMatch = Regex.IsMatch(displayName, DisplayNameRegexPattern);
            if (!doesPatternMatch)
            {
                var message = "Display Name must start with a non-whitespace character";
                throw new FormatException(message);
            }

            if (displayName.Length < DisplayNameMinLength || displayName.Length > DisplayNameMaxLength)
            {
                throw new ArgumentException($"Display Name must be between {DisplayNameMinLength} and {DisplayNameMaxLength} characters in length");
            }
        }
    }
}