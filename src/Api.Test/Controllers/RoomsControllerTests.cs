using System;
using System.Collections.Generic;
using System.Linq;
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
using ISTS.Application.Common;
using ISTS.Application.Sessions;
using ISTS.Application.Studios;

namespace ISTS.Api.Test.Controllers
{
    public class RoomsControllerTests
    {
        private readonly Mock<IRoomService> _roomService;
        private readonly Mock<IStudioService> _studioService;

        private readonly Mock<HttpContext> _httpContext;
        private readonly Mock<ClaimsIdentity> _identity;

        private readonly RoomsController _roomsController;

        public RoomsControllerTests()
        {
            _roomService = new Mock<IRoomService>();
            _studioService = new Mock<IStudioService>();

            _httpContext = new Mock<HttpContext>();
            _identity = new Mock<ClaimsIdentity>();

            _httpContext.SetupGet(c => c.User.Identity).Returns(_identity.Object);
            
            _roomsController = new RoomsController(_roomService.Object, _studioService.Object)
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
            _roomService
                .Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<RoomDto>(null));

            var result = await _roomsController.Get(Guid.NewGuid());
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Get_Returns_OkObjectResult_With_RoomDto()
        {
            var dto = new RoomDto
            {
                Id = Guid.NewGuid(),
                StudioId = Guid.NewGuid(),
                Name = "RoomName"
            };

            _roomService
                .Setup(s => s.GetAsync(It.Is<Guid>(x => x == dto.Id)))
                .Returns(Task.FromResult(dto));

            var result = await _roomsController.Get(dto.Id);
            Assert.IsType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsType<RoomDto>(okResult.Value);

            var model = okResult.Value as RoomDto;
            Assert.Equal(dto.Id, model.Id);
            Assert.Equal(dto.StudioId, model.StudioId);
            Assert.Equal(dto.Name, model.Name);
        }

        [Fact]
        public async void GetSessions_Returns_NotFound()
        {
            _roomService
                .Setup(s => s.GetSessions(It.IsAny<Guid>()))
                .Returns(Task.FromResult<List<SessionDto>>(null));

            var result = await _roomsController.GetSessions(Guid.NewGuid());
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void GetSessions_Returns_OkObjectResult_With_Collection_Of_SessionDto()
        {
            var roomId = Guid.NewGuid();
            var dtos = new List<SessionDto>
            {
                new SessionDto { RoomId = roomId },
                new SessionDto { RoomId = roomId },
                new SessionDto { RoomId = Guid.NewGuid() }
            };

            var roomSessions = dtos.Where(s => s.RoomId == roomId).ToList();

            _roomService
                .Setup(s => s.GetSessions(It.Is<Guid>(x => x == roomId)))
                .Returns(Task.FromResult(roomSessions));

            var result = await _roomsController.GetSessions(roomId);
            Assert.IsType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsType<List<SessionDto>>(okResult.Value);

            var sessions = okResult.Value as List<SessionDto>;
            Assert.Equal(roomSessions.Count, sessions.Count);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("902adcdc-1911-490e-8961-839eb2056da3")]
        public async void CreateSession_Throws_UnauthorizedAccessException(string userId)
        {
            var studio = new StudioDto
            {
                OwnerUserId = Guid.NewGuid()
            };

            var room = new RoomDto
            {
                StudioId = studio.Id
            };

            var session = new SessionDto
            {
                Id = Guid.NewGuid()
            };

            _studioService
                .Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(studio));
            
            _roomService
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(room));

            var isAuthenticated = userId != null;
            _identity.Setup(i => i.IsAuthenticated).Returns(isAuthenticated);
            _identity.Setup(i => i.Name).Returns(userId);

            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(
                () =>
                _roomsController.CreateSession(room.Id, session));

            Assert.NotNull(ex);
        }

        [Fact]
        public async void CreateSession_Returns_CreatedResult_With_SessionDto_And_Location()
        {
            var userId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var start = DateTime.Now;
            var end = start.AddHours(2);

            var studio = new StudioDto
            {
                Id = Guid.NewGuid(),
                OwnerUserId = userId
            };

            var room = new RoomDto
            {
                Id = roomId,
                StudioId = studio.Id
            };
            
            var dto = new SessionDto
            {
                RoomId = roomId,
                Schedule = new DateRangeDto
                {
                    Start = start,
                    End = end
                }
            };

            _identity.Setup(i => i.IsAuthenticated).Returns(true);
            _identity.Setup(i => i.Name).Returns(userId.ToString());

            _studioService
                .Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(studio));

            _roomService
                .Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(room));

            _roomService
                .Setup(s => s.CreateSessionAsync(It.IsAny<Guid>(), It.IsAny<SessionDto>()))
                .Returns(Task.FromResult(dto));

            var result = await _roomsController.CreateSession(room.Id, dto);
            Assert.IsType<CreatedResult>(result);

            var created = result as CreatedResult;
            Assert.IsType<SessionDto>(created.Value);

            var model = created.Value as SessionDto;
            Assert.Equal(dto.RoomId, model.RoomId);
            Assert.NotNull(model.Schedule);
            Assert.Equal(dto.Schedule.Start, model.Schedule.Start);
            Assert.Equal(dto.Schedule.End, model.Schedule.End);

            var createdLocation = created.Location;
            var expectedLocation = ApiHelper.GetResourceUri("rooms", roomId, "sessions", model.Id);
            Assert.Equal(expectedLocation, createdLocation);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async void StartSession_And_EndSession_Return_SessionDto_With_ActualStartTime_ActualEndTime(bool startSession)
        {
            var userId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var start = DateTime.Now;
            var end = start.AddHours(2);
            var actualStart = DateTime.Now;
            var actualEnd = actualStart.AddHours(2);

            var studio = new StudioDto
            {
                Id = Guid.NewGuid(),
                OwnerUserId = userId
            };

            var room = new RoomDto
            {
                Id = roomId,
                StudioId = studio.Id
            };
            
            var dto = new SessionDto
            {
                Id = Guid.NewGuid(),
                RoomId = roomId,
                Schedule = new DateRangeDto
                {
                    Start = start,
                    End = end
                },
                ActualStartTime = startSession ? actualStart : (DateTime?)null,
                ActualEndTime = !startSession ? actualEnd : (DateTime?)null
            };

            _identity.Setup(i => i.IsAuthenticated).Returns(true);
            _identity.Setup(i => i.Name).Returns(userId.ToString());

            _studioService
                .Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(studio));

            _roomService
                .Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(room));

            _roomService
                .Setup(s => s.StartSessionAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(dto));

            _roomService
                .Setup(s => s.EndSessionAsync(It.IsAny<Guid>(), It.IsAny<Guid>()))
                .Returns(Task.FromResult(dto));

            var result =
                startSession
                ? await _roomsController.StartSession(roomId, dto.Id)
                : await _roomsController.EndSession(roomId, dto.Id);

            Assert.IsType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsType<SessionDto>(okResult.Value);

            var session = okResult.Value as SessionDto;
            if (startSession)
            {
                Assert.NotNull(session.ActualStartTime);
                Assert.Equal(dto.ActualStartTime, session.ActualStartTime);
            }
            else
            {
                Assert.NotNull(session.ActualEndTime);
                Assert.Equal(dto.ActualEndTime, session.ActualEndTime);
            }
        }
        
        [Fact]
        public async void RequestSession_Returns_UnauthorizedResult()
        {
            _identity.Setup(i => i.IsAuthenticated).Returns(false);

            var result = await _roomsController.RequestSession(Guid.NewGuid(), new DateRangeDto());
            Assert.IsType<UnauthorizedResult>(result);
        }

        [Fact]
        public async void RequestSession_Returns_OkObjectResult_With_SessionRequestDto()
        {
            var userId = Guid.NewGuid();
            var roomId = Guid.NewGuid();
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(2);

            _identity.Setup(i => i.IsAuthenticated).Returns(true);
            _identity.Setup(i => i.Name).Returns(userId.ToString());

            var room = new RoomDto
            {
                Id = roomId
            };

            var expectedModel = new SessionRequestDto
            {
                Id = Guid.NewGuid(),
                RoomId = roomId,
                RequestingUserId = userId,
                RequestedTime = new DateRangeDto { Start = startTime, End = endTime }
            };

            _roomService
                .Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(room));

            _roomService
                .Setup(s => s.RequestSessionAsync(It.IsAny<SessionRequestDto>()))
                .Returns(Task.FromResult(expectedModel));

            var result = await _roomsController.RequestSession(roomId, expectedModel.RequestedTime);
            Assert.IsType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsType<SessionRequestDto>(okResult.Value);

            var actual = okResult.Value as SessionRequestDto;
            Assert.Equal(expectedModel.Id, actual.Id);
            Assert.Equal(expectedModel.RoomId, actual.RoomId);
            Assert.Equal(expectedModel.RequestingUserId, actual.RequestingUserId);
            Assert.Equal(expectedModel.RequestedTime.Start, actual.RequestedTime.Start);
            Assert.Equal(expectedModel.RequestedTime.End, actual.RequestedTime.End);
        }
    }
}