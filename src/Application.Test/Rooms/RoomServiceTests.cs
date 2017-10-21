using System;
using System.Threading.Tasks;

using AutoMapper;
using Moq;
using Xunit;

using ISTS.Application.Rooms;
using ISTS.Application.Schedules;
using ISTS.Application.Sessions;
using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;
using ISTS.Domain.Sessions;

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
        }
        
        [Fact]
        public async void CreateSession_Returns_New_RoomSessionDto_Without_Schedule()
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
        public async void CreateSession_Returns_New_RoomSessionDto_With_Schedule()
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
    }
}