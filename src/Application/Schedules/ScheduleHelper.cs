using System;

using ISTS.Application.Exceptions;
using ISTS.Domain.Schedules;

namespace ISTS.Application.Schedules
{
    public static class ScheduleHelper
    {
        public static void Reschedule(ScheduleEntry scheduleEntry, DateTime newStartDateTime, DateTime newEndDateTime)
        {
            if (scheduleEntry == null)
            {
                throw new ArgumentNullException(nameof(scheduleEntry));
            }

            if (newEndDateTime <= newStartDateTime)
            {
                throw new InvalidDateRangeException($"The end date/time must be greater than the start date/time\n\nStart Date/Time: {newStartDateTime}\nEnd Date/Time: {newEndDateTime}");
            }

            scheduleEntry.Reschedule(newStartDateTime, newEndDateTime);
        }
    }
}