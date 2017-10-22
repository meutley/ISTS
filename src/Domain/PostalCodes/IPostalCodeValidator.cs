using System;
using System.Threading.Tasks;

namespace ISTS.Domain.PostalCodes
{
    public interface IPostalCodeValidator
    {
        Task ValidateAsync(string postalCode, PostalCodeValidatorTypes validationTypes = PostalCodeValidatorTypes.Format);
    }
}