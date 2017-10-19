using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;

namespace ISTS.Domain.Studios
{
    public interface IStudioRepository
    {
        Task<Studio> CreateAsync(string name, string friendlyUrl);
        Task<IEnumerable<Studio>> GetAsync();
        Task<Studio> GetAsync(Guid id);
        Task<Studio> UpdateAsync(Guid id, string name, string friendlyUrl);
        
        Task<Room> CreateRoomAsync(Guid studioId, string name);
    }
}