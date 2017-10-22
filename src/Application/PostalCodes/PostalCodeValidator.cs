using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using ISTS.Domain.PostalCodes;

namespace ISTS.Application.PostalCodes
{
    public class PostalCodeValidator : IPostalCodeValidator
    {
        private readonly IPostalCodeRepository _postalCodeRepository;

        private static readonly string FiveDigitPattern = @"^[0-9]{5}$";
        private static readonly string FivePlusFourPattern = @"^[0-9]{5}\-[0-9]{4}$";

        public PostalCodeValidator(
            IPostalCodeRepository postalCodeRepository)
        {
            _postalCodeRepository = postalCodeRepository;
        }
        
        public async Task ValidateAsync(string postalCode, PostalCodeValidatorTypes validationTypes = PostalCodeValidatorTypes.Format)
        {
            if (validationTypes.HasFlag(PostalCodeValidatorTypes.Format))
            {
                if (!Regex.IsMatch(postalCode, FiveDigitPattern) && !Regex.IsMatch(postalCode, FivePlusFourPattern))
                {
                    throw new FormatException("Postal Code must be a 5-digit (#####) or 5-plus-4 value (#####-####)");
                }
            }

            if (validationTypes.HasFlag(PostalCodeValidatorTypes.Exists))
            {
                var entity = await _postalCodeRepository.Get(postalCode);
                if (entity == null)
                {
                    throw new PostalCodeNotFoundException();
                }
            }
        }
    }
}