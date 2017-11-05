using System;
using System.Threading.Tasks;

using AutoMapper;
using Moq;
using Xunit;

using ISTS.Application.Common;
using ISTS.Application.Rooms;
using ISTS.Application.Sessions;
using ISTS.Application.SessionRequests;
using ISTS.Domain.Common;
using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;
using ISTS.Domain.Sessions;
using ISTS.Domain.SessionRequests;

namespace ISTS.Application.Test.Rooms
{
    public class RoomServiceTests
    {
        private readonly Mock<ISessionScheduleValidator> _sessionScheduleValidator;
        private readonly Mock<IRoomRepository> _roomRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly IRoomService _roomService;

        public RoomServiceTests()
        {
            _sessionScheduleValidator = new Mock<ISessionScheduleValidator>();
            _roomRepository = new Mock<IRoomRepository>();
            _mapper = new Mock<IMapper>();

            _roomService = new RoomService(_sessionScheduleValidator.Object, _roomRepository.Object, _mapper.Object);

            _mapper
                .Setup(x => x.Map<SessionDto>(It.IsAny<Session>()))
                .Returns((Session source) =>
                {
                    return new SessionDto
                    {
                        Id = source.Id,
                        RoomId = source.RoomId,
                        Schedule =
                            source.Schedule == null
                            ? null
                            : new DateRangeDto { Start = source.Schedule.Start, End = source.Schedule.End }
                    };
                });

            _mapper
                .Setup(x => x.Map<SessionRequestDto>(It.IsAny<SessionRequest>()))
                .Returns((SessionRequest source) =>
                {
                    return new SessionRequestDto
                    {
                        Id = source.Id,
                        RoomId = source.RoomId,
                        RequestingUserId = source.RequestingUserId,
                        RequestedTime = new DateRangeDto { Start = source.RequestedStartTime, End = source.RequestedEndTime },
                        SessionRequestStatusId = source.SessionRequestStatusId,
                        RejectedReason = source.RejectedReason
                    };
                });
        }
        
        [Fact]
        public async void CreateSessionAsync_Returns_New_RoomSessionDto_Without_Schedule()
        {
            var studioId = Guid.NewGuid();
            var room = Room.Create(studioId, "Room");
            
            _roomRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(room));

            var entity = Session.Create(room.Id, null);

            _roomRepository
                .Setup(r => r.CreateSessionAsync(It.IsAny<Guid>(), It.IsAny<Session>()))
                .Returns(Task.FromResult(entity));

            var dto = new SessionDto
            {
                RoomId = room.Id
            };

            var result = await _roomService.CreateSessionAsync(room.Id, dto);

            Assert.NotNull(dto);
            Assert.Equal(room.Id, dto.RoomId);
            Assert.Null(dto.Schedule);
            Assert.Equal(studioId, room.StudioId);
        }

        [Fact]
        public async void CreateSessionAsync_Returns_New_RoomSessionDto_With_Schedule()
        {
            var studioId = Guid.NewGuid();
            var room = Room.Create(studioId, "Room");
            
            _roomRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(room));

            var schedule = DateRange.Create(DateTime.Now, DateTime.Now.AddHours(2));
            var entity = Session.Create(room.Id, null);

            _roomRepository
                .Setup(r => r.CreateSessionAsync(It.IsAny<Guid>(), It.IsAny<Session>()))
                .Returns(Task.FromResult(entity));

            _sessionScheduleValidator
                .Setup(v => v.ValidateAsync(It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<DateRange>()))
                .Returns(Task.FromResult(SessionScheduleValidatorResult.Success));

            var dto = new SessionDto
            {
                RoomId = room.Id,
                Schedule = new DateRangeDto { Start = schedule.Start, End = schedule.End }
            };

            var result = await _roomService.CreateSessionAsync(room.Id, dto);

            Assert.NotNull(dto);
            Assert.Equal(room.Id, dto.RoomId);
            Assert.NotNull(dto.Schedule);
            Assert.Equal(dto.Schedule.Start, schedule.Start);
            Assert.Equal(dto.Schedule.End, schedule.End);
            Assert.Equal(studioId, room.StudioId);
        }

        [Fact]
        public async void RequestSessionAsync_Returns_New_SessionRequestDto()
        {
            var studioId = Guid.NewGuid();
            var room = Room.Create(studioId, "Room");

            var userId = Guid.NewGuid();
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(2);
            var requestedTime = DateRange.Create(startTime, endTime);
            var entity = room.RequestSession(userId, requestedTime, _sessionScheduleValidator.Object);

            var requestedTimeDto = new DateRangeDto { Start = startTime, End = endTime };

            _roomRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(room));

            _roomRepository
                .Setup(r => r.RequestSessionAsync(It.IsAny<SessionRequest>()))
                .Returns(Task.FromResult(entity));

            _sessionScheduleValidator
                .Setup(v => v.ValidateAsync(It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<DateRange>()))
                .Returns(Task.FromResult(SessionScheduleValidatorResult.Success));

            var dto = new SessionRequestDto
            {
                RoomId = room.Id,
                RequestingUserId = userId,
                RequestedTime = new DateRangeDto { Start = startTime, End = endTime }
            };

            var result = await _roomService.RequestSessionAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(entity.Id, result.Id);
            Assert.Equal(entity.RoomId, result.RoomId);
            Assert.Equal(entity.RequestingUserId, result.RequestingUserId);
            Assert.Equal(entity.RequestedStartTime, result.RequestedTime.Start);
            Assert.Equal(entity.RequestedEndTime, result.RequestedTime.End);
        }

        [Fact]
        public async void ApproveSessionRequestAsync_Returns_Approved_SessionRequestDto()
        {
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(2);
            
            var room = Room.Create(Guid.NewGuid(), "Room");
            var request = SessionRequest.Create(Guid.NewGuid(), room.Id, startTime, endTime);
            room.SessionRequests.Add(request);

            _roomRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(room));

            var result = await _roomService.ApproveSessionRequestAsync(room.Id, request.Id);
            Assert.Equal((int)SessionRequestStatusId.Approved, result.SessionRequestStatusId);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("Need to reschedule")]
        public async void RejectSessionRequestAsync_Returns_Rejected_SessionRequestDto(string reason)
        {
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(2);
            
            var room = Room.Create(Guid.NewGuid(), "Room");
            var request = SessionRequest.Create(Guid.NewGuid(), room.Id, startTime, endTime);
            room.SessionRequests.Add(request);

            _roomRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(room));

            var result = await _roomService.RejectSessionRequestAsync(room.Id, request.Id, reason);
            Assert.Equal((int)SessionRequestStatusId.Rejected, result.SessionRequestStatusId);
            Assert.Equal(reason, result.RejectedReason);
        }
    }
}