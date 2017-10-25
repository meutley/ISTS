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

        public async Task<List<User>> GetAsync(Expression<Func<User, bool>> filter = null)
        {
            var users = _context.Users as IQueryable<User>;
            if (filter != null)
            {
                users = users.Where(filter);
            }
            
            return await users.ToListAsync();
        }

        public Task<User> GetByEmailAsync(string email)
        {
            var user = _context.Users
                .Where(u => u.Email == email);

            return user.SingleOrDefaultAsync();
        }
    }
}