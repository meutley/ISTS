using System;
using System.Threading.Tasks;

namespace ISTS.Domain.Studios
{
    public interface IStudioValidator
    {
        Task ValidateAsync(Guid? studioId, string name, string url, string postalCode);
    }
}