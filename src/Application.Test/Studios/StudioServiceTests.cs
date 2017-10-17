using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;
using Moq;
using Xunit;

using ISTS.Application.Schedules;
using ISTS.Application.Studios;
using ISTS.Domain.Exceptions;
using ISTS.Domain.Schedules;
using ISTS.Domain.Studios;

namespace ISTS.Application.Test.Studios
{
    public class StudioServiceTests
    {
        private readonly Mock<IStudioRepository>  _studioRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly IStudioService _studioService;

        public StudioServiceTests()
        {
            _studioRepository = new Mock<IStudioRepository>();
            _mapper = new Mock<IMapper>();

            _studioService = new StudioService(_studioRepository.Object, _mapper.Object);

            _mapper
                .Setup(x => x.Map<StudioDto>(It.IsAny<Studio>()))
                .Returns((Studio source) =>
                {
                    var rooms = new List<StudioRoomDto>();
                    foreach (var room in source.Rooms)
                    {
                        rooms.Add(_mapper.Object.Map<StudioRoomDto>(room));
                    }
                    
                    return new StudioDto
                    {
                        Id = source.Id,
                        Name = source.Name,
                        FriendlyUrl = source.FriendlyUrl,
                        Rooms = rooms
                    };
                });

            _mapper
                .Setup(x => x.Map<StudioRoomDto>(It.IsAny<StudioRoom>()))
                .Returns((StudioRoom source) =>
                {
                    return new StudioRoomDto
                    {
                        Id = source.Id,
                        StudioId = source.StudioId,
                        Name = source.Name
                    };
                });
        }

        [Fact]
        public async void Create_Returns_New_StudioDto()
        {
            var name = "StudioName";
            var friendlyUrl = "FriendlyUrl";
            var model = Studio.Create(name, friendlyUrl);
            
            _studioRepository
                .Setup(r => r.CreateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(model));
            
            var dto = new StudioDto { Name = name, FriendlyUrl = friendlyUrl };
            var result = await _studioService.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
            Assert.Equal(friendlyUrl, result.FriendlyUrl);
        }

        [Fact]
        public async void CreateRoom_Returns_New_StudioRoomDto()
        {
            var name = "StudioName";
            var friendlyUrl = "FriendlyUrl";
            var model = Studio.Create(name, friendlyUrl);
            
            _studioRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(model));

            var roomName = "RoomName";
            var roomModel = StudioRoom.Create(model.Id, roomName);

            _studioRepository
                .Setup(r => r.CreateRoomAsync(It.IsAny<Guid>(),It.IsAny<string>()))
                .Returns(Task.FromResult(roomModel));
            
            var dto = new StudioRoomDto { StudioId = model.Id, Name = roomName };
            var result = await _studioService.CreateRoomAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(model.Id, result.StudioId);
            Assert.Equal(roomName, result.Name);
        }
    }
}