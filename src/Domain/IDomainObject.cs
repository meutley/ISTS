using System;

namespace ISTS.Domain
{
    public interface IDomainObject
    {
        Guid Id { get; }
    }
}