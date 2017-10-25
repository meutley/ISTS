using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;
using Moq;
using Xunit;

using ISTS.Application.Rooms;
using ISTS.Application.Schedules;
using ISTS.Application.Studios;
using ISTS.Application.Studios.Search;
using ISTS.Domain.PostalCodes;
using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;
using ISTS.Domain.Studios;
using ISTS.Domain.Users;

namespace ISTS.Application.Test.Studios
{
    public class StudioServiceTests
    {
        private readonly Mock<IStudioValidator> _studioValidator;
        private readonly Mock<IStudioRepository>  _studioRepository;
        private readonly Mock<IPostalCodeRepository> _postalCodeRepository;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly IStudioService _studioService;

        private readonly Mock<IUserValidator> _userValidator;

        public StudioServiceTests()
        {
            _studioValidator = new Mock<IStudioValidator>();
            _studioRepository = new Mock<IStudioRepository>();
            _postalCodeRepository = new Mock<IPostalCodeRepository>();
            _userRepository = new Mock<IUserRepository>();
            _mapper = new Mock<IMapper>();

            _studioService = new StudioService(
                _studioValidator.Object,
                _studioRepository.Object,
                _postalCodeRepository.Object,
                _userRepository.Object,
                _mapper.Object);

            _userValidator = new Mock<IUserValidator>();

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
                        OwnerUserId = source.OwnerUserId,
                        PostalCode = source.PostalCode
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
        public async void CreateAsync_Returns_New_StudioDto()
        {
            var name = "StudioName";
            var friendlyUrl = "FriendlyUrl";
            var postalCode = "12345";
            var ownerUserId = Guid.NewGuid();
            var model = Studio.Create(name, friendlyUrl, postalCode, ownerUserId, _studioValidator.Object);
            
            _studioRepository
                .Setup(r => r.CreateAsync(It.IsAny<Studio>()))
                .Returns(Task.FromResult(model));
            
            var dto = new StudioDto { Name = name, FriendlyUrl = friendlyUrl, PostalCode = postalCode, OwnerUserId = ownerUserId };
            var result = await _studioService.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(name, result.Name);
            Assert.Equal(friendlyUrl, result.FriendlyUrl);
            Assert.Equal(ownerUserId, result.OwnerUserId);
            Assert.Equal(postalCode, result.PostalCode);
        }

        [Fact]
        public async void BuildSearchModelAsync_Returns_Unchanged_Model_For_Anonymous_User()
        {
            var postalCode = "12345";
            var distance = 50;
            
            var model = new StudioSearchModel();
            model.PostalCodeSearchCriteria = new PostalCodeSearchCriteria(postalCode, distance);

            var result = await _studioService.BuildSearchModelAsync(null, model);

            Assert.NotNull(result);
            Assert.NotNull(result.PostalCodeSearchCriteria);
            Assert.Equal(postalCode, result.PostalCodeSearchCriteria.FromPostalCode);
            Assert.Equal(distance, result.PostalCodeSearchCriteria.Distance);
        }

        [Fact]
        public async void BuildSearchModelAsync_Returns_Unchanged_Model_For_Authenticated_User()
        {
            var postalCode = "12345";
            var distance = 50;
            
            var model = new StudioSearchModel();
            model.PostalCodeSearchCriteria = new PostalCodeSearchCriteria(postalCode, distance);

            var result = await _studioService.BuildSearchModelAsync(Guid.NewGuid(), model);

            Assert.NotNull(result);
            Assert.NotNull(result.PostalCodeSearchCriteria);
            Assert.Equal(postalCode, result.PostalCodeSearchCriteria.FromPostalCode);
            Assert.Equal(distance, result.PostalCodeSearchCriteria.Distance);
        }

        [Fact]
        public async void BuildSearchModelAsync_Returns_Model_With_User_PostalCode_When_No_PostalCodeCriteria_Given()
        {
            var postalCode = "12345";
            var distance = 100;

            var users = new List<User>
            {
                User.Create(
                    _userValidator.Object,
                    "My@Email.com",
                    "DisplayName",
                    "Password",
                    postalCode)
            }.AsQueryable();

            var user = users.First();

            _userRepository
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns((Expression<Func<User, bool>> predicate) => Task.FromResult(users.Where(predicate).ToList()));

            var model = new StudioSearchModel();

            var result = await _studioService.BuildSearchModelAsync(user.Id, model);

            Assert.NotNull(result);
            Assert.NotNull(result.PostalCodeSearchCriteria);
            Assert.Equal(user.PostalCode, model.PostalCodeSearchCriteria.FromPostalCode);
            Assert.Equal(distance, model.PostalCodeSearchCriteria.Distance);
        }

        [Fact]
        public void BuildSearchModelAsync_Throws_ArgumentException_When_Anonymous_And_No_PostalCode_Criteria_Given()
        {
            var model = new StudioSearchModel();

            var ex = Assert.ThrowsAsync<ArgumentException>(() => _studioService.BuildSearchModelAsync(null, model));

            Assert.NotNull(ex);
        }

        [Fact]
        public async void CreateRoomAsyn_Returns_New_StudioRoomDto()
        {
            var name = "StudioName";
            var friendlyUrl = "FriendlyUrl";
            var postalCode = "12345";
            var ownerUserId = Guid.NewGuid();
            var model = Studio.Create(name, friendlyUrl, postalCode, ownerUserId, _studioValidator.Object);
            
            _studioRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(model));

            var roomName = "RoomName";
            var roomModel = Room.Create(model.Id, roomName);

            _studioRepository
                .Setup(r => r.CreateRoomAsync(It.IsAny<Guid>(), It.IsAny<string>()))
                .Returns(Task.FromResult(roomModel));
            
            var dto = new RoomDto { StudioId = model.Id, Name = roomName };
            var result = await _studioService.CreateRoomAsync(ownerUserId, model.Id, dto);

            Assert.NotNull(result);
            Assert.Equal(model.Id, result.StudioId);
            Assert.Equal(roomName, result.Name);
        }
    }
}