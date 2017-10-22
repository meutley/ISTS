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
        
        public async Task<PostalCode> Get(string postalCode)
        {
            var entity = await _context.PostalCodes
                .SingleOrDefaultAsync(c => c.Code == postalCode);

            return entity;
        }

        public async Task<IEnumerable<PostalCodeDistance>> GetPostalCodesWithinDistance(string fromPostalCode, decimal distance)
        {
            var results = _context.PostalCodeDistances.FromSql(
                "[dbo].[usp_GetPostalCodesWithinMiles] @fromPostalCode, @distanceInMiles",
                parameters: new object[] { fromPostalCode, distance });

            return await results.ToListAsync();
        }
    }
}