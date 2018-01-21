using System;

namespace ISTS.Application.Common
{
    public interface IDateTimeConverter
    {
        DateTime ConvertToUtc(DateTime sourceDateTime, string sourceTimeZoneId);

        DateTime ConvertFromUtc(DateTime sourceDateTime, string destinationTimeZoneId);
    }
}