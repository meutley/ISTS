using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ISTS.Domain.PostalCodes;
using ISTS.Infrastructure.Model;

namespace ISTS.Infrastructure.Repository
{
    public class PostalCodeRepository : IPostalCodeRepository
    {
        private readonly IstsContext _context;

        public PostalCodeRepository(
            IstsContext context)
        {
            _context = context;
        }
        
        public async Task<PostalCode> Get(string postalCode)
        {
            var entity = await _context.PostalCodes
                .SingleOrDefaultAsync(c => c.Code == postalCode);

            return entity;
        }
    }
}