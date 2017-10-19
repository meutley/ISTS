using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;

namespace ISTS.Domain.Studios
{
    public interface IStudioRepository
    {
        Task<Studio> CreateAsync(Studio entity);
        Task<IEnumerable<Studio>> GetAsync();
        Task<Studio> GetAsync(Guid id);
        Task<Studio> UpdateAsync(Studio entity);
        
        Task<Room> CreateRoomAsync(Guid studioId, string name);
    }
}