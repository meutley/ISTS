using System;

using ISTS.Domain.Exceptions;

namespace ISTS.Domain.Schedules
{
    public static class ScheduleValidatorHelper
    {
        public static void HandleSessionScheduleValidatorError(SessionScheduleValidatorResult validatorResult)
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