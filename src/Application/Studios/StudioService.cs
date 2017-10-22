using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using AutoMapper;

using ISTS.Application.Rooms;
using ISTS.Domain.Studios;

namespace ISTS.Application.Studios
{
    public class StudioService : IStudioService
    {
        private readonly IStudioValidator _studioUrlValidator;
        private readonly IStudioRepository _studioRepository;
        private readonly IMapper _mapper;

        public StudioService(
            IStudioValidator studioUrlValidator,
            IStudioRepository studioRepository,
            IMapper mapper)
        {
            _studioUrlValidator = studioUrlValidator;
            _studioRepository = studioRepository;
            _mapper = mapper;
        }

        public async Task<StudioDto> CreateAsync(StudioDto model)
        {
            var newEntity = Studio.Create(
                model.Name,
                model.FriendlyUrl,
                model.PostalCode,
                model.OwnerUserId,
                _studioUrlValidator);
                
            var entity = await _studioRepository.CreateAsync(newEntity);

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
            var studio = await _studioRepository.GetAsync(model.Id);
            if (studio == null)
            {
                return null;
            }

            if (studio.OwnerUserId != model.OwnerUserId)
            {
                throw new UnauthorizedAccessException();
            }

            studio.Update(model.Name, model.FriendlyUrl, model.PostalCode, _studioUrlValidator);
            var entity = await _studioRepository.UpdateAsync(studio);

            var result = _mapper.Map<StudioDto>(entity);
            return result;
        }

        public async Task<List<StudioSearchResultDto>> SearchAsync(string postalCode, int distance)
        {
            var results = await _studioRepository.SearchAsync(postalCode, distance);

            var result = results.Select(_mapper.Map<StudioSearchResultDto>).ToList();
            return result;
        }
        
        public async Task<RoomDto> CreateRoomAsync(Guid userId, Guid studioId, RoomDto model)
        {
            var studio = await _studioRepository.GetAsync(studioId);
            if (studio == null)
            {
                return null;
            }

            if (studio.OwnerUserId != userId)
            {
                throw new UnauthorizedAccessException();
            }
            
            var result = await _studioRepository.CreateRoomAsync(studioId, model.Name);

            var roomDto = _mapper.Map<RoomDto>(result);
            return roomDto;
        }
    }
}