using System;
using System.Collections.Generic;
using System.Linq;

using ISTS.Domain.Schedules;
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
        
        public Studio Create(Studio model)
        {
            var entity = new Model.Studio
            {
                Id = model.Id,
                Name = model.Name,
                FriendlyUrl = model.FriendlyUrl
            };

            _context.Studios.Add(entity);
            _context.SaveChanges();
            return model;
        }
        
        public IEnumerable<Studio> Get()
        {
            return Enumerable.Empty<Studio>();
        }

        public Studio Get(Guid id)
        {
            return null;
        }

        public StudioRoom CreateRoom(StudioRoom entity)
        {
            return null;
        }
    }
}