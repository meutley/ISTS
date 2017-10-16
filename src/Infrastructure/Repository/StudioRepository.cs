using System;
using System.Collections.Generic;
using System.Linq;

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
            return null;
        }

        public StudioRoom CreateRoom(Guid studioId, string name)
        {
            return null;
        }
    }
}