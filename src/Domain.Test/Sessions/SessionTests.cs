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
            Assert.Equal(_studioId, session.RoomId);
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
            Assert.Equal(_studioId, session.RoomId);
            Assert.NotNull(session.ScheduledTime);
            Assert.Equal(session.ScheduledTime.Start, start);
            Assert.Equal(session.ScheduledTime.End, end);
        }
    }
}