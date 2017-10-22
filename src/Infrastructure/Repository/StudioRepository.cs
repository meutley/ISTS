using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using ISTS.Domain.Rooms;
using ISTS.Domain.Studios;

namespace ISTS.Infrastructure.Repository
{
    public class StudioRepository : IStudioRepository
    {
        private readonly Model.IstsContext _context;

        public StudioRepository(
            Model.IstsContext context)
        {
            _context = context;
        }
        
        public async Task<Studio> CreateAsync(Studio entity)
        {
            await _context.Studios.AddAsync(entity);
            await _context.SaveChangesAsync();
            
            return entity;
        }
        
        public async Task<IEnumerable<Studio>> GetAsync(Func<Studio, bool> filter)
        {
            var studios = await _context.Studios.ToListAsync();
            if (filter != null)
            {
                return studios.Where(filter);
            }
            
            return studios;
        }

        public async Task<Studio> GetAsync(Guid id)
        {
            var studio = await _context.Studios
                .Include(s => s.Rooms)
                .SingleOrDefaultAsync(s => s.Id == id);

            return studio;
        }

        public async Task<Studio> GetByUrlAsync(string url)
        {
            var studio = await _context.Studios
                .Include(s => s.Rooms)
                .SingleOrDefaultAsync(s => s.FriendlyUrl == url);

            return studio;
        }

        public async Task<Studio> UpdateAsync(Studio entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return entity;
        }

        public async Task<Room> CreateRoomAsync(Guid studioId, string name)
        {
            var studio = await _context.Studios.SingleAsync(s => s.Id == studioId);
            var entity = studio.CreateRoom(name);
            
            await _context.SaveChangesAsync();

            return entity;
        }
    }
}