using System;

using ISTS.Helpers.Async;
using ISTS.Helpers.Validation;

namespace ISTS.Domain.PostalCodes
{
    public class PostalCode : IAggregateRoot
    {
        public Guid Id { get; protected set; }

        public string Code { get; protected set; }

        public static PostalCode Create(
            IPostalCodeValidator postalCodeValidator,
            string code)
        {
            ArgumentNotNullValidator.Validate(code, nameof(code));

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Code cannot be empty or whitespace");
            }

            AsyncHelper.RunSync(() => postalCodeValidator.ValidateAsync(code));
            
            var result = new PostalCode
            {
                Id = Guid.NewGuid(),
                Code = code
            };

            return result;
        }
    }
}