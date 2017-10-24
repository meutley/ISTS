using System;
using System.Collections.Generic;
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
        
        public async Task<IEnumerable<PostalCode>> GetAsync()
        {
            var entities = await _context.PostalCodes
                .ToListAsync();

            return entities;
        }
        
        public async Task<PostalCode> GetAsync(string postalCode)
        {
            var entity = await _context.PostalCodes
                .SingleOrDefaultAsync(c => c.Code == postalCode);

            return entity;
        }

        public async Task<IEnumerable<PostalCodeDistance>> GetPostalCodesWithinDistance(string postalCode, decimal distance)
        {
            var entities = _context.PostalCodeDistances.FromSql(
                "[dbo].[usp_GetPostalCodesWithinMiles] @p0, @p1",
                postalCode, distance);

            return await entities.ToListAsync();
        }
    }
}