using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ISTS.Application.Common;
using ISTS.Domain.PostalCodes;
using ISTS.Helpers.Validation;

namespace ISTS.Application.PostalCodes
{
    public class PostalCodeValidator : IPostalCodeValidator
    {
        private readonly IPostalCodeRepository _postalCodeRepository;

        private static readonly string FiveDigitPattern = @"^[0-9]{5}$";

        public PostalCodeValidator(
            IPostalCodeRepository postalCodeRepository)
        {
            _postalCodeRepository = postalCodeRepository;
        }
        
        public async Task ValidateAsync(string postalCode, PostalCodeValidatorTypes validationTypes = PostalCodeValidatorTypes.Format)
        {
            ArgumentNotNullValidator.Validate(postalCode, nameof(postalCode));
            
            if (string.IsNullOrWhiteSpace(postalCode))
            {
                throw new DataValidationException(new ArgumentException("Postal Code cannot be empty or whitespace"));
            }
            
            if (validationTypes.HasFlag(PostalCodeValidatorTypes.Format))
            {
                if (!Regex.IsMatch(postalCode, FiveDigitPattern))
                {
                    throw new DataValidationException(new PostalCodeFormatException("Postal Code must be a 5-digit (#####) value"));
                }
            }

            if (validationTypes.HasFlag(PostalCodeValidatorTypes.Exists))
            {
                var entity = await _postalCodeRepository.GetAsync(postalCode);
                if (entity == null)
                {
                    throw new DataValidationException(new PostalCodeNotFoundException());
                }
            }
        }
    }
}