using System;

using AutoMapper;

using ISTS.Application.Schedules;
using ISTS.Domain.Schedules;
using ISTS.Domain.Sessions;

namespace ISTS.Application.Sessions
{
    public class SessionService : ISessionService
    {
        private readonly ISessionScheduleValidator _sessionScheduleValidator;
        private readonly ISessionRepository _sessionRepository;
        private readonly IMapper _mapper;

        public SessionService(
            ISessionScheduleValidator sessionScheduleValidator,
            ISessionRepository sessionRepository,
            IMapper mapper)
        {
            _sessionScheduleValidator = sessionScheduleValidator;
            _sessionRepository = sessionRepository;
            _mapper = mapper;
        }
        
        public SessionDto Reschedule(Guid sessionId, DateRangeDto schedule)
        {
            var session = _sessionRepository.Get(sessionId);
            var newSchedule =
                schedule == null
                ? null
                : DateRange.Create(schedule.Start, schedule.End);

            session.Reschedule(newSchedule, _sessionScheduleValidator);
            _sessionRepository.SetSchedule(session.Id, newSchedule);

            var sessionDto = _mapper.Map<SessionDto>(session);
            return sessionDto;
        }
    }
}