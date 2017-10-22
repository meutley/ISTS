using System;

using ISTS.Helpers.Async;
using ISTS.Helpers.Validation;

namespace ISTS.Domain.PostalCodes
{
    public class PostalCode
    {
        public string Code { get; protected set; }

        public string City { get; protected set; }
        
        public string State { get; protected set; }

        public decimal Latitude { get; protected set; }

        public decimal Longitude { get; protected set; }

        public static PostalCode Create(
            IPostalCodeValidator postalCodeValidator,
            string code,
            string city,
            string state,
            decimal latitude,
            decimal longitude)
        {
            ArgumentNotNullValidator.Validate(code, nameof(code));

            if (string.IsNullOrWhiteSpace(code))
            {
                throw new ArgumentException("Code cannot be empty or whitespace");
            }

            AsyncHelper.RunSync(() => postalCodeValidator.ValidateAsync(code));
            
            var result = new PostalCode
            {
                Code = code,
                City = city,
                State = state,
                Latitude = latitude,
                Longitude = longitude
            };

            return result;
        }
    }
}