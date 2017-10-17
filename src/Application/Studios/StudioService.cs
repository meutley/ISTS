using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using AutoMapper;

using ISTS.Domain.Studios;

namespace ISTS.Application.Studios
{
    public class StudioService : IStudioService
    {
        private readonly IStudioRepository _studioRepository;
        private readonly IMapper _mapper;

        public StudioService(
            IStudioRepository studioRepository,
            IMapper mapper)
        {
            _studioRepository = studioRepository;
            _mapper = mapper;
        }

        public async Task<StudioDto> CreateAsync(StudioDto model)
        {
            var entity = await _studioRepository.CreateAsync(model.Name, model.FriendlyUrl);

            var result = _mapper.Map<StudioDto>(entity);
            return result;
        }

        public async Task<List<StudioDto>> GetAllAsync()
        {
            var entities = await _studioRepository.GetAsync();

            var result = _mapper.Map<List<StudioDto>>(entities);
            return result;
        }

        public async Task<StudioDto> GetAsync(Guid id)
        {
            var entity = await _studioRepository.GetAsync(id);
            
            var result = _mapper.Map<StudioDto>(entity);
            return result;
        }

        public async Task<StudioDto> UpdateAsync(StudioDto model)
        {
            var entity = await _studioRepository.UpdateAsync(model.Id, model.Name, model.FriendlyUrl);

            var result = _mapper.Map<StudioDto>(entity);
            return result;
        }
        
        public async Task<StudioRoomDto> CreateRoomAsync(StudioRoomDto model)
        {
            var result = await _studioRepository.CreateRoomAsync(model.StudioId, model.Name);

            var studioRoomDto = _mapper.Map<StudioRoomDto>(result);
            return studioRoomDto;
        }
    }
}