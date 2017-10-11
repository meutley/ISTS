using System;
using Xunit;

using Moq;

using ISTS.Domain.Exceptions;
using ISTS.Domain.Schedules;
using ISTS.Domain.Studios;

namespace ISTS.Domain.Tests.Studios
{
    public class StudioTests
    {
        private readonly Mock<ISessionScheduleValidator> _sessionScheduleValidatorMock;

        private static readonly Studio _studio = Studio.Create("StudioName", "StudioFriendlyUrl");

        public StudioTests()
        {
            _sessionScheduleValidatorMock = new Mock<ISessionScheduleValidator>();
        }

        [Fact]
        public void Create_Throws_ArgumentNullException_When_Name_Is_Null()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Studio.Create(null, "FriendlyUrl"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Create_Throws_ArgumentNullException_When_FriendlyUrl_Is_Null()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Studio.Create("Name", null));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Create_Returns_New_Studio()
        {
            var studio = Studio.Create("StudioName", "StudioFriendlyUrl");

            Assert.NotNull(studio);
            Assert.Equal("StudioName", studio.Name);
            Assert.Equal("StudioFriendlyUrl", studio.FriendlyUrl);
        }

        [Fact]
        public void CreateSession_Returns_New_StudioSession_With_StudioId_And_No_Schedule()
        {
            var session = _studio.CreateSession(null, _sessionScheduleValidatorMock.Object);

            Assert.NotNull(session);
            Assert.Equal(_studio.Id, session.StudioId);
            Assert.Null(session.ScheduledTime);
        }

        [Fact]
        public void CreateSession_Returns_New_StudioSession_With_StudioId_And_Schedule()
        {
            var start = DateTime.Now;
            var end = start.AddHours(2);

            var schedule = DateRange.Create(start, end);
            var session = _studio.CreateSession(schedule, _sessionScheduleValidatorMock.Object);

            Assert.NotNull(session);
            Assert.Equal(_studio.Id, session.StudioId);
            Assert.NotNull(session.ScheduledTime);
            Assert.Equal(start, session.ScheduledTime.Start);
            Assert.Equal(end, session.ScheduledTime.End);
        }

        [Fact]
        public void CreateSession_Throws_OverlappingScheduleException()
        {
            _sessionScheduleValidatorMock
                .Setup(v => v.Validate(It.IsAny<Guid>(), It.IsAny<DateRange>()))
                .Returns(SessionScheduleValidatorResult.Overlapping);

            var schedule = DateRange.Create(DateTime.Now, DateTime.Now);

            var ex = Assert.Throws<OverlappingScheduleException>(() => _studio.CreateSession(schedule, _sessionScheduleValidatorMock.Object));

            Assert.NotNull(ex);
        }

        [Fact]
        public void CreateSession_Throws_ScheduleStartAndEndMustBeProvidedException()
        {
            _sessionScheduleValidatorMock
                .Setup(v => v.Validate(It.IsAny<Guid>(), It.IsAny<DateRange>()))
                .Returns(SessionScheduleValidatorResult.StartNullEndProvided);

            var schedule = DateRange.Create(DateTime.Now, DateTime.Now);

            var ex = Assert.Throws<ScheduleStartAndEndMustBeProvidedException>(() => _studio.CreateSession(schedule, _sessionScheduleValidatorMock.Object));

            Assert.NotNull(ex);
        }

        [Fact]
        public void CreateSession_Throws_ScheduleEndMustBeGreaterThanStartException()
        {
            _sessionScheduleValidatorMock
                .Setup(v => v.Validate(It.IsAny<Guid>(), It.IsAny<DateRange>()))
                .Returns(SessionScheduleValidatorResult.StartGreaterThanOrEqualToEnd);

            var schedule = DateRange.Create(DateTime.Now, DateTime.Now);

            var ex = Assert.Throws<ScheduleEndMustBeGreaterThanStartException>(() => _studio.CreateSession(schedule, _sessionScheduleValidatorMock.Object));

            Assert.NotNull(ex);
        }
    }
}