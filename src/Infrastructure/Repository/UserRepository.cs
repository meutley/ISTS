using System;
using System.Threading.Tasks;
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
    }
}