using System;
using System.Collections.Generic;
using System.Linq;

using ISTS.Domain.Exceptions;
using ISTS.Domain.Schedules;
using ISTS.Domain.Studios;

namespace ISTS.Application.Schedules
{
    public class SessionScheduleValidator : ISessionScheduleValidator
    {
        private readonly IStudioRepository _studioRepository;

        public SessionScheduleValidator(
            IStudioRepository studioRepository)
        {
            _studioRepository = studioRepository;
        }

        public SessionScheduleValidatorResult Validate(Guid studioId, Guid? sessionId, DateRange schedule)
        {
            if (schedule != null)
            {
                if (schedule.Start >= schedule.End)
                {
                    throw new ScheduleEndMustBeGreaterThanStartException();
                }

                var studioSchedule =
                    _studioRepository
                    .GetSchedule(studioId, schedule.Start, schedule.End)
                    .Where(s => s.SessionId != sessionId)
                    .ToList();

                if (studioSchedule.Any() && DoesScheduleOverlap(schedule, studioSchedule))
                {
                    throw new OverlappingScheduleException();
                }
            }

            return SessionScheduleValidatorResult.Success;
        }

        private bool DoesScheduleOverlap(DateRange schedule, List<StudioSessionSchedule> studioSchedule)
        {
            var overlap =
                studioSchedule
                .Where(s => s.Schedule != null && DoDateRangesOverlap(schedule, s.Schedule));

            return overlap.Any();
        }

        private bool DoDateRangesOverlap(DateRange left, DateRange right)
        {
            return
                (left.Start >= right.Start && left.Start < right.End)
                || (left.End >= right.Start && left.End < right.End)
                || (left.Start >= right.Start && left.End <= right.End);
        }
    }
}