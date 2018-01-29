using System;

using ISTS.Application.Common;
using ISTS.Application.Sessions;

namespace ISTS.Api.Helpers
{
    public static class SessionDtoExtensions
    {
        public static void ConvertScheduleToUtc(this SessionDto target, string sourceTimeZoneId)
        {
            if (target.Schedule != null)
            {
                target.Schedule.ConvertToUtc(sourceTimeZoneId);
            }
        }

        public static void ConvertScheduleFromUtc(this SessionDto target, string destinationTimeZoneId)
        {
            if (target.Schedule != null)
            {
                target.Schedule.ConvertFromUtc(destinationTimeZoneId);
            }
        }
    }
}