using System;

namespace ISTS.Domain.PostalCodes
{
    public class PostalCodeDistance
    {
        public decimal Distance { get; protected set; }
        
        public string Code { get; protected set; }

        public static PostalCodeDistance Create(
            string code,
            decimal distance)
        {
            return new PostalCodeDistance
            {
                Code = code,
                Distance = distance
            };
        }
    }
}