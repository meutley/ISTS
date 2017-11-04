using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Moq;
using Xunit;

using ISTS.Api.Controllers;
using ISTS.Api.Helpers;
using ISTS.Application.Rooms;
using ISTS.Application.Studios;

namespace ISTS.Api.Test.Controllers
{
    public class StudiosControllerTests
    {
        private readonly Mock<IStudioService> _studioService;

        private readonly Mock<HttpContext> _httpContext;
        private readonly Mock<ClaimsIdentity> _identity;

        private readonly StudiosController _studiosController;

        public StudiosControllerTests()
        {
            _studioService = new Mock<IStudioService>();

            _httpContext = new Mock<HttpContext>();
            _identity = new Mock<ClaimsIdentity>();

            _httpContext.SetupGet(c => c.User.Identity).Returns(_identity.Object);
            
            _studiosController = new StudiosController(_studioService.Object)
            {
                ControllerContext = new ControllerContext
                {
                    HttpContext = _httpContext.Object
                }
            };
        }

        [Fact]
        public async void Get_Returns_NotFound()
        {
            _studioService
                .Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<StudioDto>(null));

            var result = await _studiosController.Get(Guid.NewGuid());
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Get_Returns_OkObjectResult_With_StudioDto()
        {
            var dto = new StudioDto
            {
                Id = Guid.NewGuid(),
                Name = "Studio",
                FriendlyUrl = "FriendlyUrl",
                PostalCode = "11111"
            };
            
            _studioService
                .Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(dto));

            var result = await _studiosController.Get(dto.Id);
            Assert.IsType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsType<StudioDto>(okResult.Value);

            var model = okResult.Value as StudioDto;
            Assert.Equal(dto.Id, model.Id);
            Assert.Equal(dto.Name, model.Name);
            Assert.Equal(dto.FriendlyUrl, model.FriendlyUrl);
            Assert.Equal(dto.PostalCode, model.PostalCode);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("902adcdc-1911-490e-8961-839eb2056da3")]
        public async void Post_Throws_UnauthorizedAccessException(string userId)
        {
            var model = new StudioDto
            {
                OwnerUserId = Guid.NewGuid()
            };

            var isAuthenticated = userId != null;
            _identity.Setup(i => i.IsAuthenticated).Returns(isAuthenticated);
            _identity.Setup(i => i.Name).Returns(userId);

            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() => _studiosController.Post(model));

            Assert.NotNull(ex);
        }

        [Fact]
        public async void Post_Returns_CreatedResult_With_StudioDto_And_Location()
        {
            var model = new StudioDto
            {
                Id = Guid.NewGuid(),
                Name = "Studio",
                FriendlyUrl = "FriendlyUrl",
                PostalCode = "11111",
                OwnerUserId = Guid.NewGuid()
            };

            _identity.Setup(i => i.IsAuthenticated).Returns(true);
            _identity.Setup(i => i.Name).Returns(model.OwnerUserId.ToString());

            _studioService
                .Setup(s => s.CreateAsync(It.IsAny<StudioDto>()))
                .Returns(Task.FromResult(model));

            var result = await _studiosController.Post(model);
            Assert.IsType<CreatedResult>(result);

            var created = result as CreatedResult;
            Assert.IsType<StudioDto>(created.Value);

            var studio = created.Value as StudioDto;
            Assert.Equal(model.Id, studio.Id);
            Assert.Equal(model.Name, studio.Name);
            Assert.Equal(model.FriendlyUrl, studio.FriendlyUrl);
            Assert.Equal(model.PostalCode, studio.PostalCode);
            Assert.Equal(model.OwnerUserId, studio.OwnerUserId);

            var expectedLocation = ApiHelper.GetResourceUri("studios", model.Id);
            Assert.Equal(expectedLocation, created.Location);
        }

        [Fact]
        public async void Put_Returns_OkObjectResult_With_Updated_Model()
        {
            var model = new StudioDto
            {
                Id = Guid.NewGuid(),
                Name = "Studio",
                FriendlyUrl = "FriendlyUrl",
                PostalCode = "11111",
                OwnerUserId = Guid.NewGuid()
            };

            var updatedModel = new StudioDto
            {
                Id = model.Id,
                Name = "Studio_Updated",
                FriendlyUrl = "FriendlyUrl_Updated",
                PostalCode = "22222",
                OwnerUserId = model.OwnerUserId
            };

            _identity.Setup(i => i.IsAuthenticated).Returns(true);
            _identity.Setup(i => i.Name).Returns(model.OwnerUserId.ToString());

            _studioService
                .Setup(s => s.UpdateAsync(It.IsAny<StudioDto>()))
                .Returns(Task.FromResult(updatedModel));

            var result = await _studiosController.Put(model);
            Assert.IsType<OkObjectResult>(result);

            var ok = result as OkObjectResult;
            Assert.IsType<StudioDto>(ok.Value);

            var studio = ok.Value as StudioDto;
            Assert.Equal(updatedModel.Id, studio.Id);
            Assert.Equal(updatedModel.Name, studio.Name);
            Assert.Equal(updatedModel.FriendlyUrl, studio.FriendlyUrl);
            Assert.Equal(updatedModel.PostalCode, studio.PostalCode);
            Assert.Equal(updatedModel.OwnerUserId, studio.OwnerUserId);
        }

        [Fact]
        public async void CreateRoom_Returns_CreatedResult_With_RoomDto_And_Location()
        {
            var userId = Guid.NewGuid();
            var studioId = Guid.NewGuid();

            var studio = new StudioDto
            {
                OwnerUserId = userId
            };
            
            var model = new RoomDto
            {
                Id = Guid.NewGuid(),
                Name = "Studio"
            };

            _identity.Setup(i => i.IsAuthenticated).Returns(true);
            _identity.Setup(i => i.Name).Returns(userId.ToString());

            _studioService
                .Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(studio));

            _studioService
                .Setup(s => s.CreateRoomAsync(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<RoomDto>()))
                .Returns(Task.FromResult(model));

            var result = await _studiosController.CreateRoom(studioId, model);
            Assert.IsType<CreatedResult>(result);

            var created = result as CreatedResult;
            Assert.IsType<RoomDto>(created.Value);

            var room = created.Value as RoomDto;
            Assert.Equal(model.Id, room.Id);
            Assert.Equal(model.Name, room.Name);

            var expectedLocation = ApiHelper.GetResourceUri("studios", studioId, "rooms", model.Id);
            Assert.Equal(expectedLocation, created.Location);
        }

        [Fact]
        public async void GetRooms_Returns_List_Of_RoomDto()
        {
            var models = new List<RoomDto>
            {
                new RoomDto(),
                new RoomDto(),
                new RoomDto()
            };

            _studioService
                .Setup(s => s.GetRoomsAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(models));

            var result = await _studiosController.GetRooms(Guid.NewGuid());
            Assert.IsType<OkObjectResult>(result);

            var ok = result as OkObjectResult;
            Assert.IsType<List<RoomDto>>(ok.Value);

            var rooms = ok.Value as List<RoomDto>;
            Assert.Equal(models.Count, rooms.Count);
        }
    }
}