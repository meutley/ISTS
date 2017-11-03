using System;

namespace ISTS.Domain.PostalCodes
{
    public class PostalCodeDistance
    {
        public string Code { get; protected set; }
        
        public decimal Distance { get; protected set; }
        
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