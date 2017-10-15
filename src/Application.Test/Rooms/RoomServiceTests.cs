using System;

using AutoMapper;
using Moq;
using Xunit;

using ISTS.Application.Rooms;
using ISTS.Application.Schedules;
using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;

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
                .Setup(x => x.Map<RoomSessionDto>(It.IsAny<RoomSession>()))
                .Returns((RoomSession source) =>
                {
                    return new RoomSessionDto
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
        public void CreateSession_Returns_New_RoomSessionDto_Without_Schedule()
        {
            var studioId = Guid.NewGuid();
            var room = Room.Create(studioId, "Room");
            
            _roomRepository
                .Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns(room);

            var entity = RoomSession.Create(room.Id, null);

            _roomRepository
                .Setup(r => r.CreateSession(It.IsAny<Guid>(), It.IsAny<RoomSession>()))
                .Returns(entity);

            var dto = new RoomSessionDto
            {
                RoomId = room.Id
            };

            var result = _roomService.CreateSession(dto);

            Assert.NotNull(dto);
            Assert.Equal(room.Id, dto.RoomId);
            Assert.Null(dto.Schedule);
        }

        [Fact]
        public void CreateSession_Returns_New_RoomSessionDto_With_Schedule()
        {
            var studioId = Guid.NewGuid();
            var room = Room.Create(studioId, "Room");
            
            _roomRepository
                .Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns(room);

            var schedule = DateRange.Create(DateTime.Now, DateTime.Now.AddHours(2));
            var entity = RoomSession.Create(room.Id, null);

            _roomRepository
                .Setup(r => r.CreateSession(It.IsAny<Guid>(), It.IsAny<RoomSession>()))
                .Returns(entity);

            _sessionScheduleValidator
                .Setup(v => v.Validate(It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<DateRange>()))
                .Returns(SessionScheduleValidatorResult.Success);

            var dto = new RoomSessionDto
            {
                RoomId = room.Id,
                Schedule = new DateRangeDto { Start = schedule.Start, End = schedule.End }
            };

            var result = _roomService.CreateSession(dto);

            Assert.NotNull(dto);
            Assert.Equal(room.Id, dto.RoomId);
            Assert.NotNull(dto.Schedule);
            Assert.Equal(dto.Schedule.Start, schedule.Start);
            Assert.Equal(dto.Schedule.End, schedule.End);
        }
    }
}