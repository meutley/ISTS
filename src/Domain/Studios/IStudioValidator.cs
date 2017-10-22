using System;
using System.Threading.Tasks;

namespace ISTS.Domain.Studios
{
    public interface IStudioValidator
    {
        Task<StudioValidatorResult> ValidateAsync(Guid? studioId, string name, string url, string postalCode);
    }
}