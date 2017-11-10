using System;
using System.Threading.Tasks;

namespace ISTS.Domain.Sessions
{
    public interface ISessionRepository
    {
        Task<Session> GetAsync(Guid id);
    }
}