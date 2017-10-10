using System;

using ISTS.Domain.Studios;

namespace ISTS.Application.Studios
{
    public interface IStudioRepository
    {
        Studio Get(Guid studioId);
    }
}