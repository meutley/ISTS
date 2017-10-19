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
        
        public async Task<Studio> CreateAsync(string name, string friendlyUrl)
        {
            var entity = Studio.Create(name, friendlyUrl);

            await _context.Studios.AddAsync(entity);
            await _context.SaveChangesAsync();
            
            return entity;
        }
        
        public async Task<IEnumerable<Studio>> GetAsync()
        {
            return Enumerable.Empty<Studio>().ToList();
        }

        public async Task<Studio> GetAsync(Guid id)
        {
            var studio = await _context.Studios
                .Include(s => s.Rooms)
                .FirstOrDefaultAsync(s => s.Id == id);

            return studio;
        }

        public async Task<Studio> UpdateAsync(Guid id, string name, string friendlyUrl)
        {
            var studio = await GetAsync(id);
            studio.Update(name, friendlyUrl);
            await _context.SaveChangesAsync();

            return studio;
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