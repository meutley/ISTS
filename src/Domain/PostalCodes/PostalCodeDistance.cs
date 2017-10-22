using System;

namespace ISTS.Domain.PostalCodes
{
    public class PostalCodeDistance
    {
        public decimal Distance { get; protected set; }

        public string Code { get; protected set; }

        public static PostalCodeDistance Create(decimal distance, string code)
        {
            return new PostalCodeDistance
            {
                Distance = distance,
                Code = code
            };
        }
    }
}