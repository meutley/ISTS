using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

using AutoMapper;

using ISTS.Application.Rooms;
using ISTS.Application.Studios.Search;
using ISTS.Domain.PostalCodes;
using ISTS.Domain.Studios;
using ISTS.Helpers.Validation;

namespace ISTS.Application.Studios
{
    public class StudioService : IStudioService
    {
        private readonly IStudioValidator _studioUrlValidator;
        private readonly IStudioRepository _studioRepository;
        private readonly IPostalCodeRepository _postalCodeRepository;
        private readonly IMapper _mapper;

        public StudioService(
            IStudioValidator studioUrlValidator,
            IStudioRepository studioRepository,
            IPostalCodeRepository postalCodeRepository,
            IMapper mapper)
        {
            _studioUrlValidator = studioUrlValidator;
            _studioRepository = studioRepository;
            _postalCodeRepository = postalCodeRepository;
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

        public async Task<List<StudioSearchResultDto>> SearchAsync(StudioSearchModel searchModel)
        {
            ArgumentNotNullValidator.Validate(searchModel, nameof(searchModel));
            
            var studios = await _studioRepository.GetAsync();

            var postalCodes = await GetPostalCodes(searchModel);
            studios = studios.Where(s => postalCodes.Any(p => p.Code == s.PostalCode));

            var inMemoryStudios = studios.ToList();
            var result = inMemoryStudios.Select(s =>
                new StudioSearchResultDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    FriendlyUrl = s.FriendlyUrl,
                    OwnerUserId = s.OwnerUserId,
                    PostalCode = s.PostalCode,
                    Distance = (double)postalCodes.First(p => p.Code == s.PostalCode).Distance
                })
                .OrderBy(s => s.Distance)
                .ToList();

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

        private async Task<List<PostalCodeDistance>> GetPostalCodes(
            StudioSearchModel searchModel)
        {
            ArgumentNotNullValidator.Validate(searchModel.PostalCodeSearchCriteria, nameof(searchModel.PostalCodeSearchCriteria));

            var criteria = searchModel.PostalCodeSearchCriteria;
            
            var postalCodesWithinDistance =
                (await _postalCodeRepository.GetPostalCodesWithinDistance(
                    criteria.FromPostalCode,
                    criteria.Distance)).ToList();

            return postalCodesWithinDistance;
        }
    }
}