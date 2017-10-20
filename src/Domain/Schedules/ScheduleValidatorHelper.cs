using System;

namespace ISTS.Domain.Schedules
{
    internal static class ScheduleValidatorHelper
    {
        internal static void HandleSessionScheduleValidatorError(SessionScheduleValidatorResult validatorResult)
        {
            switch (validatorResult)
            {
                case SessionScheduleValidatorResult.Success:
                    return;
                case SessionScheduleValidatorResult.Overlapping:
                    throw new OverlappingScheduleException();
                case SessionScheduleValidatorResult.StartGreaterThanOrEqualToEnd:
                    throw new ScheduleEndMustBeGreaterThanStartException();
            }
        }
    }
}