using System;

namespace ISTS.Domain.Schedules
{
    public interface ISessionScheduleValidator
    {
        SessionScheduleValidatorResult Validate(Guid studioId, DateRange schedule);
    }
}