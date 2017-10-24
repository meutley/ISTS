using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ISTS.Domain.PostalCodes
{
    public interface IPostalCodeRepository
    {
        Task<IEnumerable<PostalCode>> GetAsync();
        Task<PostalCode> GetAsync(string postalCode);
        Task<IEnumerable<PostalCodeDistance>> GetPostalCodesWithinDistance(string postalCode, decimal distance);
    }
}