using System;

namespace ISTS.Domain.Schedules
{
    public interface ISessionScheduleValidator
    {
        SessionScheduleValidatorResult Validate(Guid roomId, Guid? sessionId, DateRange schedule);
    }
}