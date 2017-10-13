using System;

using ISTS.Domain.Studios;

namespace ISTS.Application.Studios
{
    public interface IStudioService
    {
        StudioSessionDto CreateSession(Guid studioId, StudioSessionDto session);
    }
}