using System;
using System.Collections.Generic;
using System.Linq;

using Moq;
using Xunit;

using ISTS.Application.Schedules;
using ISTS.Domain.Exceptions;
using ISTS.Domain.Schedules;
using ISTS.Domain.Studios;

namespace ISTS.Application.Test.Schedules
{
    public class SessionScheduleValidatorTests
    {
        private static readonly Guid StudioId = Guid.NewGuid();

        private static readonly DateTime Start = new DateTime(2017, 1, 1, 5, 0, 0);
        private static readonly DateTime End = new DateTime(2017, 1, 1, 7, 0, 0);

        private static readonly IEnumerable<StudioSessionSchedule> StudioSchedule =
            new List<StudioSessionSchedule>
            {
                StudioSessionSchedule.Create(Guid.NewGuid(), DateRange.Create(Start, End)),
                StudioSessionSchedule.Create(Guid.NewGuid(), DateRange.Create(Start.AddDays(1), End.AddDays(1))),
                StudioSessionSchedule.Create(Guid.NewGuid(), DateRange.Create(Start.AddDays(2), End.AddDays(2))),
            };

        private readonly Mock<IStudioRepository> _studioRepositoryMock;
        private readonly SessionScheduleValidator _validator;

        public SessionScheduleValidatorTests()
        {
            _studioRepositoryMock = new Mock<IStudioRepository>();

            _validator = new SessionScheduleValidator(_studioRepositoryMock.Object);
        }

        [Fact]
        public void Validate_Should_Return_Success()
        {
            _studioRepositoryMock
                .Setup(r => r.GetSchedule(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Enumerable.Empty<StudioSessionSchedule>());

            var schedule = DateRange.Create(DateTime.Now, DateTime.Now.AddHours(2));

            var result = _validator.Validate(StudioId, null, schedule);

            Assert.Equal(SessionScheduleValidatorResult.Success, result);
        }

        [Fact]
        public void Validate_Should_Throw_Exception_When_Start_Equals_End()
        {
            _studioRepositoryMock
                .Setup(r => r.GetSchedule(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Enumerable.Empty<StudioSessionSchedule>());

            var start = DateTime.Now;
            var end = start;
            var schedule = DateRange.Create(start, end);

            var ex = Assert.Throws<ScheduleEndMustBeGreaterThanStartException>(() => _validator.Validate(StudioId, null, schedule));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Validate_Should_Throw_Exception_When_Start_Greater_Than_End()
        {
            _studioRepositoryMock
                .Setup(r => r.GetSchedule(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(Enumerable.Empty<StudioSessionSchedule>());

            var start = DateTime.Now;
            var end = start.AddHours(-2);
            var schedule = DateRange.Create(start, end);

            var ex = Assert.Throws<ScheduleEndMustBeGreaterThanStartException>(() => _validator.Validate(StudioId, null, schedule));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Validate_Should_Throw_Exception_When_Schedule_End_Overlaps()
        {
            _studioRepositoryMock
                .Setup(r => r.GetSchedule(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(StudioSchedule);

            var start = Start.AddHours(-1);
            var end = Start.AddMinutes(30);
            var schedule = DateRange.Create(start, end);

            var ex = Assert.Throws<OverlappingScheduleException>(() => _validator.Validate(StudioId, null, schedule));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Validate_Should_Throw_Exception_When_Schedule_Start_Overlaps()
        {
            _studioRepositoryMock
                .Setup(r => r.GetSchedule(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(StudioSchedule);

            var start = Start.AddHours(1);
            var end = Start.AddHours(3);
            var schedule = DateRange.Create(start, end);

            var ex = Assert.Throws<OverlappingScheduleException>(() => _validator.Validate(StudioId, null, schedule));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Validate_Should_Return_Success_When_Schedule_Overlaps_With_Same_SessionId()
        {
            _studioRepositoryMock
                .Setup(r => r.GetSchedule(It.IsAny<Guid>(), It.IsAny<DateTime>(), It.IsAny<DateTime>()))
                .Returns(StudioSchedule);

            var start = Start.AddMinutes(-30);
            var end = End.AddMinutes(-30);
            var schedule = DateRange.Create(start, end);

            var sessionId = StudioSchedule.First().SessionId;
            var result = _validator.Validate(StudioId, sessionId, schedule);

            Assert.Equal(SessionScheduleValidatorResult.Success, result);
        }
    }
}