using System;

namespace ISTS.Application.Studios
{
    public interface IStudioService
    {
        StudioDto Create(StudioDto studio);
        StudioDto Get(Guid id);
    }
}