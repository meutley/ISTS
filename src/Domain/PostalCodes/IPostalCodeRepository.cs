using System;
using System.Threading.Tasks;

namespace ISTS.Domain.PostalCodes
{
    public interface IPostalCodeRepository
    {
        Task<PostalCode> Get(string postalCode);
    }
}