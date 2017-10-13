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
                .Setup(x => x.Map<StudioSessionDto>(It.IsAny<StudioSession>()))
                .Returns((StudioSession source) =>
                {
                    return new StudioSessionDto
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
        public void CreateSession_Returns_StudioSessionDto_Without_Schedule()
        {
            var studioId = Guid.NewGuid();
            var studio = Studio.Create(studioId, "Name", "Url");

            var studioSession = new StudioSessionDto
            {
                StudioId = studioId
            };

            _studioRepository
                .Setup(r => r.Get(It.IsAny<Guid>()))
                .Returns(studio);

            var result = _studioService.CreateSession(studioId, studioSession);

            Assert.NotNull(result);
            Assert.Null(result.ScheduledTime);
            Assert.Equal(studioId, studioSession.StudioId);

            _studioRepository.Verify(r => r.Get(It.IsAny<Guid>()), Times.Once);
            _studioRepository.Verify(r => r.CreateSession(It.IsAny<Guid>(), It.IsAny<DateRange>()), Times.Once);
        }
    }
}