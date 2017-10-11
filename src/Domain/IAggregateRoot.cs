using System;

namespace ISTS.Domain
{
    public interface IAggregateRoot
    {
        Guid Id { get; }
    }
}