using System;

namespace ISTS.Application.Studios.Search
{
    public class PostalCodeSearchCriteria
    {
        public string FromPostalCode { get; protected set; }
        
        public decimal Distance { get; protected set; }

        public PostalCodeSearchCriteria(
            string fromPostalCode,
            decimal distance)
        {
            this.FromPostalCode = fromPostalCode;
            this.Distance = distance;
        }
    }
}