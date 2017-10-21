using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ISTS.Domain.Users;
using ISTS.Infrastructure.Model;

namespace ISTS.Infrastructure.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly IstsContext _context;

        public UserRepository(
            IstsContext context)
        {
            _context = context;
        }
        
        public async Task<User> CreateAsync(User entity)
        {
            await _context.Users.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<IEnumerable<User>> GetAsync(Func<User, bool> filter = null)
        {
            var users = _context.Users;
            if (filter != null)
            {
                return users.Where(filter);
            }

            return await users.ToListAsync();
        }
    }
}