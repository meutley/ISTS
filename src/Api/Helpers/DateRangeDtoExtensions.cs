using System;

using ISTS.Application.Common;

namespace ISTS.Api.Helpers
{
    public static class DateRangeDtoExtensions
    {
        public static void ConvertToUtc(this DateRangeDto target, string sourceTimeZoneId)
        {
            target.Start = DateTimeConverter.ConvertToUtc(target.Start, sourceTimeZoneId);
            target.End = DateTimeConverter.ConvertToUtc(target.End, sourceTimeZoneId);
        }

        public static void ConvertFromUtc(this DateRangeDto target, string destinationTimeZoneId)
        {
            target.Start = DateTimeConverter.ConvertFromUtc(target.Start, destinationTimeZoneId);
            target.End = DateTimeConverter.ConvertFromUtc(target.End, destinationTimeZoneId);
        }
    }
}