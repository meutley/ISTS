using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ISTS.Domain.Common;
using ISTS.Domain.PostalCodes;

namespace ISTS.Domain.Studios
{
    public class StudioValidator : IStudioValidator
    {
        private readonly IPostalCodeValidator _postalCodeValidator;
        private readonly IStudioRepository _studioRepository;

        private static readonly int NameMinLength = 5;
        private static readonly int NameMaxLength = 50;
        private static readonly int UrlMinLength = 5;
        private static readonly int UrlMaxLength = 25;
        private static readonly string ValidUrlCharactersRegexPattern = @"^[a-zA-Z]([a-zA-Z0-9\-_]+)?$";

        public StudioValidator(
            IPostalCodeValidator postalCodeValidator,
            IStudioRepository studioRepository)
        {
            _postalCodeValidator = postalCodeValidator;
            _studioRepository = studioRepository;
        }
        
        public async Task ValidateAsync(Guid? studioId, string name, string url, string postalCode)
        {
            await _postalCodeValidator.ValidateAsync(
                postalCode,
                PostalCodeValidatorTypes.Format | PostalCodeValidatorTypes.Exists);
        
            var urlValidationResult = await ValidateUrlAsync(studioId, url);
            var nameValidationResult = ValidateName(name);
        }

        private bool ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new DomainValidationException(new ArgumentException("Name cannot be empty or whitespace"));
            }

            if (name.Length < NameMinLength || name.Length > NameMaxLength)
            {
                throw new DomainValidationException(new ArgumentException($"Name must be between {NameMinLength} and {NameMaxLength} characters in length"));
            }

            return true;
        }

        private async Task<bool> ValidateUrlAsync(Guid? studioId, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new DomainValidationException(new ArgumentException(string.Format("{0} is required", nameof(url))));
            }

            if (url.Length < UrlMinLength || url.Length > UrlMaxLength)
            {
                throw new DomainValidationException(new ArgumentException($"URL must be between {UrlMinLength} and {UrlMaxLength} characters in length"));
            }
            
            var entity = await _studioRepository.GetByUrlAsync(url);
            var urlAlreadyExists = entity != null && entity.Id != studioId;
                
            if (urlAlreadyExists)
            {
                throw new DomainValidationException(new StudioUrlInUseException(string.Format("The given url is already in use: {0}", url)));
            }

            var doesUrlMatchPattern = Regex.IsMatch(url, ValidUrlCharactersRegexPattern);
            if (!doesUrlMatchPattern)
            {
                var message = "URL must start with a letter, and can only contain letters, numbers, hyphens and underscores";

                throw new DomainValidationException(new UriFormatException(message));
            }

            return true;
        }
    }
}