using System;
using System.Threading.Tasks;

using ISTS.Domain.Common;

namespace ISTS.Domain.Sessions
{
    public interface ISessionScheduleValidator
    {
        Task<SessionScheduleValidatorResult> ValidateAsync(Guid roomId, Guid? sessionId, DateRange schedule);
    }
}