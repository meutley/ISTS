using System;

using AutoMapper;
using Moq;
using Xunit;

using ISTS.Application.Schedules;
using ISTS.Application.Sessions;
using ISTS.Domain.Exceptions;
using ISTS.Domain.Schedules;
using ISTS.Domain.Sessions;

namespace ISTS.Application.Test.Sessions
{
    public class SessionServiceTests
    {
        private readonly Mock<ISessionScheduleValidator> _sessionScheduleValidator;
        private readonly Mock<ISessionRepository>  _sessionRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly ISessionService _sessionService;

        public SessionServiceTests()
        {
            _sessionScheduleValidator = new Mock<ISessionScheduleValidator>();
            _sessionRepository = new Mock<ISessionRepository>();
            _mapper = new Mock<IMapper>();

            _sessionService = new SessionService(_sessionScheduleValidator.Object, _sessionRepository.Object, _mapper.Object);

            _mapper
                .Setup(x => x.Map<SessionDto>(It.IsAny<Session>()))
                .Returns((Session source) =>
                {
                    return new SessionDto
                    {
                        Id = source.Id,
                        StudioId = source.StudioId,
                        ScheduledTime =
                            source.ScheduledTime == null
                            ? null
                            : new DateRangeDto
                            {
                                Start = source.ScheduledTime.Start,
                                End = source.ScheduledTime.End
                            }
                    };
                });
        }
        
        [Fact]
        public void Reschedule_Calls_Repository_And_Returns_Session_With_New_Schedule()
        {
            var sessionId = Guid.NewGuid();
            var session = Session.Create(sessionId, null, _sessionScheduleValidator.Object);

            var newSchedule = new DateRangeDto
            {
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(2)
            };

            _sessionRepository
                .Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns(session);

            var result = _sessionService.Reschedule(sessionId, newSchedule);

            Assert.NotNull(result);
            Assert.NotNull(result.ScheduledTime);
            Assert.Equal(result.ScheduledTime.Start, newSchedule.Start);
            Assert.Equal(result.ScheduledTime.End, newSchedule.End);

            _sessionRepository.Verify(r => r.Get(It.IsAny<Guid>()), Times.Once);
            _sessionRepository.Verify(r => r.SetSchedule(It.IsAny<Guid>(), It.IsAny<DateRange>()), Times.Once);
        }

        [Fact]
        public void Reschedule_Should_Throw_OverlappingScheduleException()
        {
            var sessionId = Guid.NewGuid();
            var session = Session.Create(sessionId, null, _sessionScheduleValidator.Object);

            var newSchedule = new DateRangeDto
            {
                Start = DateTime.Now,
                End = DateTime.Now.AddHours(2)
            };

            _sessionRepository
                .Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns(session);

            _sessionScheduleValidator
                .Setup(v => v.Validate(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<DateRange>()))
                .Throws(new OverlappingScheduleException());

            var ex = Assert.Throws<OverlappingScheduleException>(() => _sessionService.Reschedule(sessionId, newSchedule));

            Assert.NotNull(ex);
        }
    }
}