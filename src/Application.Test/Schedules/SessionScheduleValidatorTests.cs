using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Moq;
using Xunit;

using ISTS.Application.Schedules;
using ISTS.Domain.Exceptions;
using ISTS.Domain.Schedules;
using ISTS.Domain.Rooms;

namespace ISTS.Application.Test.Schedules
{
    public class SessionScheduleValidatorTests
    {
        private static readonly Guid RoomId = Guid.NewGuid();

        private static readonly DateTime Start = new DateTime(2017, 1, 1, 5, 0, 0);
        private static readonly DateTime End = new DateTime(2017, 1, 1, 7, 0, 0);

        private static readonly IEnumerable<RoomSessionSchedule> RoomSchedule =
            new List<RoomSessionSchedule>
            {
                RoomSessionSchedule.Create(Guid.NewGuid(), DateRange.Create(Start, End)),
                RoomSessionSchedule.Create(Guid.NewGuid(), DateRange.Create(Start.AddDays(1), End.AddDays(1))),
                RoomSessionSchedule.Create(Guid.NewGuid(), DateRange.Create(Start.AddDays(2), End.AddDays(2))),
            };

        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly SessionScheduleValidator _validator;

        public SessionScheduleValidatorTests()
        {
            _roomRepositoryMock = new Mock<IRoomRepository>();

            _validator = new SessionScheduleValidator(_roomRepositoryMock.Object);
        }

        [Fact]
        public async void Validate_Should_Return_Success()
        {
            _roomRepositoryMock
                .Setup(r => r.GetScheduleAsync(It.IsAny<Guid>(), It.IsAny<DateRange>()))
                .Returns(Task.FromResult(Enumerable.Empty<RoomSessionSchedule>()));

            var schedule = DateRange.Create(DateTime.Now, DateTime.Now.AddHours(2));

            var result = await _validator.ValidateAsync(RoomId, null, schedule);

            Assert.Equal(SessionScheduleValidatorResult.Success, result);
        }

        [Fact]
        public void Validate_Should_Throw_Exception_When_Start_Equals_End()
        {
            _roomRepositoryMock
                .Setup(r => r.GetScheduleAsync(It.IsAny<Guid>(), It.IsAny<DateRange>()))
                .Returns(Task.FromResult(Enumerable.Empty<RoomSessionSchedule>()));

            var start = DateTime.Now;
            var end = start;
            var schedule = DateRange.Create(start, end);

            var ex = Assert.ThrowsAsync<ScheduleEndMustBeGreaterThanStartException>(() => _validator.ValidateAsync(RoomId, null, schedule));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Validate_Should_Throw_Exception_When_Start_Greater_Than_End()
        {
            _roomRepositoryMock
                .Setup(r => r.GetScheduleAsync(It.IsAny<Guid>(), It.IsAny<DateRange>()))
                .Returns(Task.FromResult(Enumerable.Empty<RoomSessionSchedule>()));

            var start = DateTime.Now;
            var end = start.AddHours(-2);
            var schedule = DateRange.Create(start, end);

            var ex = Assert.ThrowsAsync<ScheduleEndMustBeGreaterThanStartException>(() => _validator.ValidateAsync(RoomId, null, schedule));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Validate_Should_Throw_Exception_When_Schedule_End_Overlaps()
        {
            _roomRepositoryMock
                .Setup(r => r.GetScheduleAsync(It.IsAny<Guid>(), It.IsAny<DateRange>()))
                .Returns(Task.FromResult(RoomSchedule));

            var start = Start.AddHours(-1);
            var end = Start.AddMinutes(30);
            var schedule = DateRange.Create(start, end);

            var ex = Assert.ThrowsAsync<OverlappingScheduleException>(() => _validator.ValidateAsync(RoomId, null, schedule));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Validate_Should_Throw_Exception_When_Schedule_Start_Overlaps()
        {
            _roomRepositoryMock
                .Setup(r => r.GetScheduleAsync(It.IsAny<Guid>(), It.IsAny<DateRange>()))
                .Returns(Task.FromResult(RoomSchedule));

            var start = Start.AddHours(1);
            var end = Start.AddHours(3);
            var schedule = DateRange.Create(start, end);

            var ex = Assert.ThrowsAsync<OverlappingScheduleException>(() => _validator.ValidateAsync(RoomId, null, schedule));

            Assert.NotNull(ex);
        }

        [Fact]
        public async void Validate_Should_Return_Success_When_Schedule_Overlaps_With_Same_SessionId()
        {
            _roomRepositoryMock
                .Setup(r => r.GetScheduleAsync(It.IsAny<Guid>(), It.IsAny<DateRange>()))
                .Returns(Task.FromResult(RoomSchedule));

            var start = Start.AddMinutes(-30);
            var end = End.AddMinutes(-30);
            var schedule = DateRange.Create(start, end);

            var sessionId = RoomSchedule.First().SessionId;
            var result = await _validator.ValidateAsync(RoomId, sessionId, schedule);

            Assert.Equal(SessionScheduleValidatorResult.Success, result);
        }
    }
}