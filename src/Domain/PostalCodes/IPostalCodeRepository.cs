using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISTS.Domain.PostalCodes
{
    public interface IPostalCodeRepository
    {
        Task<PostalCode> Get(string postalCode);
        Task<IEnumerable<PostalCodeDistance>> GetPostalCodesWithinDistance(string fromPostalCode, decimal distance);
    }
}