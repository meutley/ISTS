using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ISTS.Domain.Studios;

namespace ISTS.Application.Studios
{
    public interface IStudioService
    {
        Task<StudioDto> CreateAsync(StudioDto model);
        Task<List<StudioDto>> GetAllAsync();
        Task<StudioDto> GetAsync(Guid id);
        Task<StudioDto> UpdateAsync(StudioDto model);
        
        Task<StudioRoomDto> CreateRoomAsync(StudioRoomDto model);
    }
}