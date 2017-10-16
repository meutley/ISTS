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

        public StudioDto Create(string name, string friendlyUrl)
        {
            var entity = _studioRepository.Create(name, friendlyUrl);

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
            // var entity = _studioRepository.Get(id);
            var entity = _studioRepository.Create("My Studio", "mystudio");
            
            var result = _mapper.Map<StudioDto>(entity);
            return result;
        }
        
        public StudioRoomDto CreateRoom(Guid studioId, string name)
        {
            var studio = _studioRepository.Get(studioId);
            var model = studio.CreateRoom(name);

            var result = _studioRepository.CreateRoom(model.StudioId, model.Name);

            var studioRoomDto = _mapper.Map<StudioRoomDto>(result);
            return studioRoomDto;
        }
    }
}