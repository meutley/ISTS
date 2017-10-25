using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;

namespace ISTS.Domain.Studios
{
    public interface IStudioRepository
    {
        Task<Studio> CreateAsync(Studio entity);
        Task<List<Studio>> GetAsync(Expression<Func<Studio, bool>> filter = null);
        Task<Studio> GetAsync(Guid id);
        Task<Studio> GetByUrlAsync(string url);
        Task<Studio> UpdateAsync(Studio entity);
        
        Task<Room> CreateRoomAsync(Guid studioId, string name);
    }
}