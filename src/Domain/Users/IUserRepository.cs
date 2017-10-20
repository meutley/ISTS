using System;
using System.Threading.Tasks;

namespace ISTS.Domain.Users
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User entity);
    }
}