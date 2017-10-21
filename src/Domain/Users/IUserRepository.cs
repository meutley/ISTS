using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ISTS.Domain.Users
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User entity);
        Task<IEnumerable<User>> GetAsync(Func<User, bool> filter = null);
    }
}