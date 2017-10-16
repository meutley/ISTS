using System;
using System.Collections.Generic;
using System.Linq;
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
        
        public Studio Create(string name, string friendlyUrl)
        {
            var entity = Studio.Create(name, friendlyUrl);

            _context.Studios.Add(entity);
            _context.SaveChanges();
            
            return entity;
        }
        
        public IEnumerable<Studio> Get()
        {
            return Enumerable.Empty<Studio>();
        }

        public Studio Get(Guid id)
        {
            var studio = _context.Studios
                .Include(s => s.Rooms)
                .FirstOrDefault(s => s.Id == id);

            return studio;
        }

        public Studio Update(Guid id, string name, string friendlyUrl)
        {
            var studio = Get(id);
            studio.Update(name, friendlyUrl);
            _context.SaveChanges();

            return studio;
        }

        public StudioRoom CreateRoom(Guid studioId, string name)
        {
            var studio = _context.Studios.Single(s => s.Id == studioId);
            var entity = studio.CreateRoom(name);
            
            _context.SaveChanges();

            return entity;
        }
    }
}