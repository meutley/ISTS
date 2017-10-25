using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ISTS.Domain.Users
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User entity);
        Task<List<User>> GetAsync(Expression<Func<User, bool>> filter = null);
        Task<User> GetByEmailAsync(string email);
    }
}