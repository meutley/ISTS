using System;

namespace ISTS.Application.Common
{
    public class DateTimeConverter : IDateTimeConverter
    {
        public DateTime ConvertToUtc(DateTime sourceDateTime, string sourceTimeZoneId)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(sourceTimeZoneId);
            return TimeZoneInfo.ConvertTimeToUtc(sourceDateTime, timeZoneInfo);
        }

        public DateTime ConvertFromUtc(DateTime sourceDateTime, string destinationTimeZoneId)
        {
            var timeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById(destinationTimeZoneId);
            return TimeZoneInfo.ConvertTimeFromUtc(sourceDateTime, timeZoneInfo);
        }
    }
}