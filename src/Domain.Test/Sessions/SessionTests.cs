using System;
using Xunit;

using Moq;

using ISTS.Domain.Exceptions;
using ISTS.Domain.Sessions;
using ISTS.Domain.Schedules;

namespace ISTS.Domain.Tests.Sessions
{
    public class SessionTests
    {
        private readonly Mock<ISessionScheduleValidator> _sessionScheduleValidatorMock;

        private static readonly Guid _studioId = Guid.NewGuid();

        public SessionTests()
        {
            _sessionScheduleValidatorMock = new Mock<ISessionScheduleValidator>();

            _sessionScheduleValidatorMock
                .Setup(v => v.Validate(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateRange>()))
                .Returns(SessionScheduleValidatorResult.Success);
        }

        [Fact]
        public void Create_Returns_New_Session_Without_Schedule()
        {
            var session = Session.Create(_studioId, null, _sessionScheduleValidatorMock.Object);

            Assert.NotNull(session);
            Assert.Equal(_studioId, session.StudioId);
            Assert.Null(session.ScheduledTime);
        }

        [Fact]
        public void Create_Returns_New_StudioSession_With_Schedule()
        {
            var start = DateTime.Now;
            var end = start.AddHours(2);
            
            var schedule = DateRange.Create(start, end);
            var session = Session.Create(_studioId, schedule, _sessionScheduleValidatorMock.Object);

            Assert.NotNull(session);
            Assert.Equal(_studioId, session.StudioId);
            Assert.NotNull(session.ScheduledTime);
            Assert.Equal(session.ScheduledTime.Start, start);
            Assert.Equal(session.ScheduledTime.End, end);
        }

        [Fact]
        public void Reschedule_Throws_OverlappingScheduleException()
        {
            _sessionScheduleValidatorMock
                .Setup(v => v.Validate(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateRange>()))
                .Returns(SessionScheduleValidatorResult.Success);

            var schedule = DateRange.Create(DateTime.Now, DateTime.Now);
            var newSchedule = DateRange.Create(DateTime.Now.AddDays(2), DateTime.Now.AddDays(2));
            var session = Session.Create(_studioId, schedule, _sessionScheduleValidatorMock.Object);

            _sessionScheduleValidatorMock
                .Setup(v => v.Validate(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateRange>()))
                .Returns(SessionScheduleValidatorResult.Overlapping);

            var ex = Assert.Throws<OverlappingScheduleException>(() => session.Reschedule(newSchedule, _sessionScheduleValidatorMock.Object));

            Assert.NotNull(ex);
        }

        [Fact]
        public void CreateSession_Throws_ScheduleEndMustBeGreaterThanStartException()
        {
            _sessionScheduleValidatorMock
                .Setup(v => v.Validate(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateRange>()))
                .Returns(SessionScheduleValidatorResult.Success);

            var schedule = DateRange.Create(DateTime.Now, DateTime.Now);
            var newSchedule = DateRange.Create(DateTime.Now.AddDays(2), DateTime.Now.AddDays(2));
            var session = Session.Create(_studioId, schedule, _sessionScheduleValidatorMock.Object);

            _sessionScheduleValidatorMock
                .Setup(v => v.Validate(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateRange>()))
                .Returns(SessionScheduleValidatorResult.StartGreaterThanOrEqualToEnd);

            var ex = Assert.Throws<ScheduleEndMustBeGreaterThanStartException>(() => session.Reschedule(newSchedule, _sessionScheduleValidatorMock.Object));

            Assert.NotNull(ex);
        }
    }
}