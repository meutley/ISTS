using System;

using AutoMapper;

using ISTS.Application.Schedules;
using ISTS.Domain.Schedules;
using ISTS.Domain.Studios;

namespace ISTS.Application.Studios
{
    public class StudioService
    {
        private readonly ISessionScheduleValidator _sessionScheduleValidator;
        private readonly IStudioRepository _studioRepository;
        private readonly IMapper _mapper;

        public StudioService(
            ISessionScheduleValidator sessionScheduleValidator,
            IStudioRepository studioRepository,
            IMapper mapper)
        {
            _sessionScheduleValidator = sessionScheduleValidator;
            _studioRepository = studioRepository;
            _mapper = mapper;
        }
        
        public StudioSessionDto CreateSession(Guid studioId, StudioSessionDto session)
        {
            var studio = _studioRepository.Get(studioId);

            var schedule =
                session == null
                ? null
                : DateRange.Create(session.ScheduledTime.Start, session.ScheduledTime.End);

            var studioSession = studio.CreateSession(schedule, _sessionScheduleValidator);
            var result = _studioRepository.CreateSession(studio.Id, studioSession.ScheduledTime);

            var studioSessionDto = _mapper.Map<StudioSessionDto>(studioSession);
            return studioSessionDto;
        }
    }
}