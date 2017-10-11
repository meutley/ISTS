using System;
using System.Collections.Generic;

namespace ISTS.Domain.Studios
{
    public interface IStudioRepository
    {
        IEnumerable<Studio> Get();
        Studio Get(Guid id);
    }
}