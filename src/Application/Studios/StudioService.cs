using System;
using System.Collections.Generic;
using System.Linq;

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

        public StudioDto Create(StudioDto model)
        {
            var entity = _studioRepository.Create(model.Name, model.FriendlyUrl);

            var result = _mapper.Map<StudioDto>(entity);
            return result;
        }

        public List<StudioDto> GetAll()
        {
            var entities = _studioRepository.Get().ToList();

            var result = _mapper.Map<List<StudioDto>>(entities);
            return result;
        }

        public StudioDto Get(Guid id)
        {
            var entity = _studioRepository.Get(id);
            
            var result = _mapper.Map<StudioDto>(entity);
            return result;
        }

        public StudioDto Update(StudioDto model)
        {
            var entity = _studioRepository.Update(model.Id, model.Name, model.FriendlyUrl);

            var result = _mapper.Map<StudioDto>(entity);
            return result;
        }
        
        public StudioRoomDto CreateRoom(StudioRoomDto model)
        {
            var result = _studioRepository.CreateRoom(model.StudioId, model.Name);

            var studioRoomDto = _mapper.Map<StudioRoomDto>(result);
            return studioRoomDto;
        }
    }
}