using System;
using System.Threading.Tasks;

namespace ISTS.Domain.Schedules
{
    public interface ISessionScheduleValidator
    {
        Task<SessionScheduleValidatorResult> ValidateAsync(Guid roomId, Guid? sessionId, DateRange schedule);
    }
}