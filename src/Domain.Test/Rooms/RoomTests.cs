using System;
using System.Threading.Tasks;

using Moq;
using Xunit;

using ISTS.Domain.Exceptions;
using ISTS.Domain.Rooms;
using ISTS.Domain.Schedules;

namespace ISTS.Domain.Tests.Rooms
{
    public class RoomTests
    {
        private readonly Mock<ISessionScheduleValidator> _sessionScheduleValidator;

        private static readonly Guid StudioId = Guid.NewGuid();
        private static readonly Room Room = Room.Create(StudioId, "RoomName");

        public RoomTests()
        {
            _sessionScheduleValidator = new Mock<ISessionScheduleValidator>();

            _sessionScheduleValidator
                .Setup(v => v.ValidateAsync(It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<DateRange>()))
                .Returns(Task.FromResult(SessionScheduleValidatorResult.Success));
        }
        
        [Fact]
        public void CreateSession_Returns_New_RoomSession_Without_Schedule()
        {
            var session = Room.CreateSession(null, _sessionScheduleValidator.Object);

            Assert.NotNull(session);
            Assert.Equal(Room.Id, session.RoomId);
            Assert.Null(session.Schedule);
        }

        [Fact]
        public void RescheduleSession_Returns_RoomSession_With_New_Schedule()
        {
            var schedule = DateRange.Create(DateTime.Now, DateTime.Now.AddHours(2));
            var session = Room.CreateSession(schedule, _sessionScheduleValidator.Object);

            Assert.NotNull(session);
            Assert.Equal(Room.Id, session.RoomId);
            Assert.NotNull(session.Schedule);
            Assert.Equal(schedule.Start, session.Schedule.Start);
            Assert.Equal(schedule.End, session.Schedule.End);
        }

        [Fact]
        public void StartSession_Sets_ActualStartTime()
        {
            var time = DateTime.Now;
            var session = Room.CreateSession(null, _sessionScheduleValidator.Object);
            Room.StartSession(session.Id, time);

            Assert.NotNull(session);
            Assert.Equal(Room.Id, session.RoomId);
            Assert.NotNull(session.ActualStartTime);
            Assert.Equal(time, session.ActualStartTime);
        }

        [Fact]
        public void StartSession_Throws_SessionAlreadyStartedException()
        {
            var time = DateTime.Now;
            var session = Room.CreateSession(null, _sessionScheduleValidator.Object);
            Room.StartSession(session.Id, time);
            
            var ex = Assert.Throws<SessionAlreadyStartedException>(() => Room.StartSession(session.Id, time));
            Assert.NotNull(ex);
        }

        [Fact]
        public void EndSession_Sets_ActualEndTime()
        {
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(2);
            var session = Room.CreateSession(null, _sessionScheduleValidator.Object);
            Room.StartSession(session.Id, startTime);
            Room.EndSession(session.Id, endTime);

            Assert.NotNull(session);
            Assert.Equal(Room.Id, session.RoomId);
            Assert.NotNull(session.ActualStartTime);
            Assert.Equal(startTime, session.ActualStartTime);
            Assert.NotNull(session.ActualEndTime);
            Assert.Equal(endTime, session.ActualEndTime);
        }

        [Fact]
        public void EndSession_Throws_SessionNotStartException()
        {
            var endTime = DateTime.Now;
            var session = Room.CreateSession(null, _sessionScheduleValidator.Object);
            
            var ex = Assert.Throws<SessionNotStartedException>(() => Room.EndSession(session.Id, endTime));
            Assert.NotNull(ex);
        }
    }
}