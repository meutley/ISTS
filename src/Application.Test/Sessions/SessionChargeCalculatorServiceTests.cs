using System;
using System.Threading.Tasks;

using Moq;
using Xunit;

using ISTS.Application.Common;
using ISTS.Application.Sessions;
using ISTS.Domain.Rooms;
using ISTS.Domain.Common;
using ISTS.Domain.Sessions;

namespace ISTS.Application.Test.Sessions
{
    public class SessionChargeCalculatorServiceTests
    {
        private readonly Mock<ISessionRepository> _sessionRepository;
        private readonly Mock<IRoomRepository> _roomRepository;

        private readonly ISessionChargeCalculatorService _sessionChargeCalculatorService;

        private static Guid StudioId = Guid.NewGuid();
        private static Guid RoomId = Guid.NewGuid();
        private static Guid RoomFunctionId = Guid.NewGuid();
        private static DateTime Start = DateTime.Now;
        private static DateTime End = Start.AddHours(2);
        private static DateRange Schedule = DateRange.Create(Start, End);

        public SessionChargeCalculatorServiceTests()
        {
            _sessionRepository = new Mock<ISessionRepository>();
            _roomRepository = new Mock<IRoomRepository>();

            _sessionChargeCalculatorService = new SessionChargeCalculatorService(
                _sessionRepository.Object,
                _roomRepository.Object);
        }

        [Fact]
        public async void CalculateTotalCharge_Returns_Zero_When_RoomFunctionId_Is_Null()
        {           
            var session = Session.Create(
                RoomId,
                Schedule,
                null);

            session.SetActualStartTime(Start);
            session.SetActualEndTime(End);
            
            _sessionRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(session));

            var result = await _sessionChargeCalculatorService.CalculateTotalCharge(session.Id);
            Assert.Equal(0, result);
        }

        [Fact]
        public async void CalculateTotalCharge_Throws_When_Session_Not_Started()
        {
            var session = Session.Create(
                RoomId,
                Schedule,
                RoomFunctionId);
            
            _sessionRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(session));

            var ex = await Assert.ThrowsAsync<DataValidationException>(() => _sessionChargeCalculatorService.CalculateTotalCharge(session.Id));
            Assert.IsType<DataValidationException>(ex);
            Assert.NotNull(ex.InnerException);
            Assert.IsType<SessionNotStartedException>(ex.InnerException);
        }

        [Fact]
        public async void CalculateTotalCharge_Throws_When_Session_Not_Ended()
        {
            var session = Session.Create(
                RoomId,
                Schedule,
                RoomFunctionId);
            
            session.SetActualStartTime(Start);

            _sessionRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(session));

            var ex = await Assert.ThrowsAsync<DataValidationException>(() => _sessionChargeCalculatorService.CalculateTotalCharge(session.Id));
            Assert.IsType<DataValidationException>(ex);
            Assert.NotNull(ex.InnerException);
            Assert.IsType<SessionNotEndedException>(ex.InnerException);
        }

        [Fact]
        public async void CalculateTotalCharge_Hourly_Returns_MinimumCharge_If_Total_Less_Than_Minimum()
        {
            var minimum = 50;
            
            var room = Room.Create(StudioId, "Room");
            var function = room.AddRoomFunction("Vocal tracking", "Tracking vocals for a song");
            function.SetBillingRate(BillingRate.HourlyType, 10, minimum);

            var session = Session.Create(
                RoomId,
                Schedule,
                function.Id);
            
            session.SetActualStartTime(Start);
            session.SetActualEndTime(End);

            _sessionRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(session));

            _roomRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(room));

            var result = await _sessionChargeCalculatorService.CalculateTotalCharge(session.Id);
            Assert.Equal(minimum, result);
        }

        [Fact]
        public async void CalculateTotalCharge_Hourly_Returns_Total()
        {
            var minimum = 25;
            var expectedTotal = 2 * 25;
            
            var room = Room.Create(StudioId, "Room");
            var function = room.AddRoomFunction("Vocal tracking", "Tracking vocals for a song");
            function.SetBillingRate(BillingRate.HourlyType, 25, minimum);

            var session = Session.Create(
                RoomId,
                Schedule,
                function.Id);
            
            session.SetActualStartTime(Start);
            session.SetActualEndTime(End);

            _sessionRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(session));

            _roomRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(room));

            var result = await _sessionChargeCalculatorService.CalculateTotalCharge(session.Id);
            Assert.Equal(expectedTotal, result);
        }

        [Fact]
        public async void CalculateTotalCharge_FlatRate_Returns_FlatRate_Total()
        {
            var expectedTotal = 100;
            
            var room = Room.Create(StudioId, "Room");
            var function = room.AddRoomFunction("Vocal tracking", "Tracking vocals for a song");
            function.SetBillingRate(BillingRate.FlatRateType, expectedTotal, 0);

            var session = Session.Create(
                RoomId,
                Schedule,
                function.Id);
            
            session.SetActualStartTime(Start);
            session.SetActualEndTime(End);

            _sessionRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(session));

            _roomRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(room));

            var result = await _sessionChargeCalculatorService.CalculateTotalCharge(session.Id);
            Assert.Equal(expectedTotal, result);
        }

        [Fact]
        public async void CalculateTotalCharge_None_Returns_Zero()
        {
            var expectedTotal = 0;
            
            var room = Room.Create(StudioId, "Room");
            var function = room.AddRoomFunction("Vocal tracking", "Tracking vocals for a song");

            var session = Session.Create(
                RoomId,
                Schedule,
                function.Id);
            
            session.SetActualStartTime(Start);
            session.SetActualEndTime(End);

            _sessionRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(session));

            _roomRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(room));

            var result = await _sessionChargeCalculatorService.CalculateTotalCharge(session.Id);
            Assert.Equal(expectedTotal, result);
        }
    }
}