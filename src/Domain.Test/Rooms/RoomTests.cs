using System;
using System.Threading.Tasks;

using Moq;
using Xunit;

using ISTS.Domain.Rooms;
using ISTS.Domain.Common;
using ISTS.Domain.Sessions;
using ISTS.Domain.SessionRequests;

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
            var schedule = DateRange.Create(time, time.AddHours(2));
            var session = Room.CreateSession(schedule, _sessionScheduleValidator.Object);
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
            var schedule = DateRange.Create(time, time.AddHours(2));
            var session = Room.CreateSession(schedule, _sessionScheduleValidator.Object);
            Room.StartSession(session.Id, time);
            
            var ex = Assert.Throws<SessionAlreadyStartedException>(() => Room.StartSession(session.Id, time));
            Assert.NotNull(ex);
        }

        [Fact]
        public void EndSession_Sets_ActualEndTime()
        {
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(2);
            var schedule = DateRange.Create(startTime, endTime);
            var session = Room.CreateSession(schedule, _sessionScheduleValidator.Object);
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
            var schedule = DateRange.Create(endTime, endTime.AddHours(2));
            var session = Room.CreateSession(schedule, _sessionScheduleValidator.Object);
            
            var ex = Assert.Throws<SessionNotStartedException>(() => Room.EndSession(session.Id, endTime));
            Assert.NotNull(ex);
        }

        [Fact]
        public void RequestSession_Returns_SessionRequest_Model()
        {
            var userId = Guid.NewGuid();
            var roomFunctionId = Guid.NewGuid();
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(2);
            var requestedTime = DateRange.Create(startTime, endTime);

            var model = Room.RequestSession(userId, requestedTime, roomFunctionId, _sessionScheduleValidator.Object);

            Assert.NotNull(model);
            Assert.Equal(userId, model.RequestingUserId);
            Assert.Equal(roomFunctionId, model.RoomFunctionId);
            Assert.Equal(startTime, model.RequestedStartTime);
            Assert.Equal(endTime, model.RequestedEndTime);
            Assert.Equal(requestedTime, model.RequestedTime);
            Assert.Equal((int)SessionRequestStatusId.Pending, model.SessionRequestStatusId);
        }

        [Theory]
        [InlineData("Approve", "", (int)SessionRequestStatusId.Approved)]
        [InlineData("Reject", "Need to reschedule", (int)SessionRequestStatusId.Rejected)]
        public void ApproveReject_SessionRequest_Returns_SessionRequest_With_Updated_Status(
            string type,
            string reason,
            int expectedStatusId)
        {
            var userId = Guid.NewGuid();
            var roomFunctionId = Guid.NewGuid();
            var startTime = DateTime.Now;
            var endTime = startTime.AddHours(2);
            var requestedTime = DateRange.Create(startTime, endTime);

            _sessionScheduleValidator
                .Setup(v => v.ValidateAsync(It.IsAny<Guid>(), It.IsAny<Guid?>(), It.IsAny<DateRange>()))
                .Returns(Task.FromResult(SessionScheduleValidatorResult.Success));

            var request = Room.RequestSession(userId, requestedTime, roomFunctionId, _sessionScheduleValidator.Object);
            Assert.Equal((int)SessionRequestStatusId.Pending, request.SessionRequestStatusId);

            SessionRequest modifiedRequest = null;
            switch (type)
            {
                case "Approve":
                    modifiedRequest = Room.ApproveSessionRequest(request.Id, _sessionScheduleValidator.Object);
                    break;
                case "Reject":
                    modifiedRequest = Room.RejectSessionRequest(request.Id, reason);
                    break;
            }

            Assert.Equal(expectedStatusId, modifiedRequest.SessionRequestStatusId);
        }
    }
}