using System;

using AutoMapper;
using Moq;
using Xunit;

using ISTS.Application.Schedules;
using ISTS.Application.Studios;
using ISTS.Domain.Exceptions;
using ISTS.Domain.Schedules;
using ISTS.Domain.Studios;

namespace ISTS.Application.Test.Studios
{
    public class StudioServiceTests
    {
        private readonly Mock<ISessionScheduleValidator> _sessionScheduleValidator;
        private readonly Mock<IStudioRepository>  _studioRepository;
        private readonly Mock<IMapper> _mapper;

        private readonly IStudioService _studioService;

        public StudioServiceTests()
        {
            _sessionScheduleValidator = new Mock<ISessionScheduleValidator>();
            _studioRepository = new Mock<IStudioRepository>();
            _mapper = new Mock<IMapper>();

            _studioService = new StudioService(_sessionScheduleValidator.Object, _studioRepository.Object, _mapper.Object);

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
    }
}