using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using AutoMapper;
using Moq;
using Xunit;

using ISTS.Application.Rooms;
using ISTS.Application.Schedules;
using ISTS.Application.Studios;
using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;
using ISTS.Domain.Studios;

namespace ISTS.Application.Test.Studios
{
    public class StudioServiceTests
    {
        private readonly Mock<IStudioValidator> _studioValidator;
        private readonly Mock<IStudioRepository>  _studioRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly IStudioService _studioService;

        public StudioServiceTests()
        {
            _studioValidator = new Mock<IStudioValidator>();
            _studioRepository = new Mock<IStudioRepository>();
            _mapper = new Mock<IMapper>();

            _studioService = new StudioService(_studioValidator.Object, _studioRepository.Object, _mapper.Object);

            _studioValidator
                .Setup(v => v.ValidateAsync(It.IsAny<Guid?>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(StudioValidatorResult.Success));

            _mapper
                .Setup(x => x.Map<StudioDto>(It.IsAny<Studio>()))
                .Returns((Studio source) =>
                {
                    var rooms = new List<RoomDto>();
                    foreach (var room in source.Rooms)
                    {
                        rooms.Add(_mapper.Object.Map<RoomDto>(room));
                    }
                    
                    return new StudioDto
                    {
                        Id = source.Id,
                        Name = source.Name,
                        FriendlyUrl = source.FriendlyUrl,
                        Rooms = rooms,
                        OwnerUserId = source.OwnerUserId
                    };
                });

            _mapper
                .Setup(x => x.Map<RoomDto>(It.IsAny<Room>()))
                .Returns((Room source) =>
                {
                    return new RoomDto
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
            var ownerUserId = Guid.NewGuid();
            var model = Studio.Create(name, friendlyUrl, ownerUserId, _studioValidator.Object);
            
            _studioRepository
                .Setup(r => r.CreateAsync(It.IsAny<Studio>()))
                .Returns(Task.FromResult(model));
            
            var dto = new StudioDto { Name = name, FriendlyUrl = friendlyUrl, OwnerUserId = ownerUserId };
            var result = await _studioService.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
            Assert.Equal(friendlyUrl, result.FriendlyUrl);
            Assert.Equal(ownerUserId, result.OwnerUserId);
        }

        [Fact]
        public async void CreateRoom_Returns_New_StudioRoomDto()
        {
            var name = "StudioName";
            var friendlyUrl = "FriendlyUrl";
            var ownerUserId = Guid.NewGuid();
            var model = Studio.Create(name, friendlyUrl, ownerUserId, _studioValidator.Object);
            
            _studioRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(model));

            var roomName = "RoomName";
            var roomModel = Room.Create(model.Id, roomName);

            _studioRepository
                .Setup(r => r.CreateRoomAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(roomModel));
            
            var dto = new RoomDto { StudioId = model.Id, Name = roomName };
            var result = await _studioService.CreateRoomAsync(model.Id, dto);

            Assert.NotNull(result);
            Assert.Equal(model.Id, result.StudioId);
            Assert.Equal(roomName, result.Name);
        }
    }
}