using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using ISTS.Application.Rooms;
using ISTS.Application.Studios.Search;
using ISTS.Domain.Studios;

namespace ISTS.Application.Studios
{
    public interface IStudioService
    {
        Task<StudioSearchModel> BuildSearchModelAsync(Guid? userId, StudioSearchModel model);
        
        Task<StudioDto> CreateAsync(StudioDto model);
        Task<List<StudioDto>> GetAllAsync();
        Task<StudioDto> GetAsync(Guid id);
        Task<StudioDto> GetByFriendlyUrlAsync(string friendlyUrl);
        Task<StudioDto> UpdateAsync(StudioDto model);
        Task<List<StudioSearchResultDto>> SearchAsync(StudioSearchModel searchModel);
        
        Task<RoomDto> CreateRoomAsync(Guid userId, Guid studioId, RoomDto model);
        Task<List<RoomDto>> GetRoomsAsync(Guid id);
    }
}