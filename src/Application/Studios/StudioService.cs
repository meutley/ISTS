using System;

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
            var model = Studio.Create(name, friendlyUrl);
            var entity = _studioRepository.Create(model);

            var result = _mapper.Map<StudioDto>(entity);
            return result;
        }
        
        public StudioRoomDto CreateRoom(Guid studioId, string name)
        {
            var studio = _studioRepository.Get(studioId);

            var studioRoom = studio.CreateRoom(name);
            var result = _studioRepository.CreateRoom(studioRoom);

            var studioRoomDto = _mapper.Map<StudioRoomDto>(result);
            return studioRoomDto;
        }
    }
}