using System;

using ISTS.Domain.Exceptions;
using ISTS.Domain.Schedules;

namespace ISTS.Application.Schedules
{
    public class SessionScheduleValidator : ISessionScheduleValidator
    {
        public SessionScheduleValidatorResult Validate(Guid studioId, Guid? sessionId, DateRange schedule)
        {
            if (schedule != null)
            {
                if (schedule.Start >= schedule.End)
                {
                    throw new ScheduleEndMustBeGreaterThanStartException();
                }
            }

            return SessionScheduleValidatorResult.Success;
        }
    }
}