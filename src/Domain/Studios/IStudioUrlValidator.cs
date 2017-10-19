using System;
using System.Threading.Tasks;

namespace ISTS.Domain.Studios
{
    public interface IStudioUrlValidator
    {
        Task<StudioUrlValidatorResult> ValidateAsync(Guid? studioId, string url);
    }
}