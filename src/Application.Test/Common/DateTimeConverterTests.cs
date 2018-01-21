using System;

using Xunit;

using ISTS.Application.Common;

namespace ISTS.Application.Test.Common
{
    public class DateTimeConverterTests
    {
        private readonly IDateTimeConverter _dateTimeConverter;
        
        public DateTimeConverterTests()
        {
            _dateTimeConverter = new DateTimeConverter();
        }
        
        [Fact]
        public void ConvertToUtc_From_CentralStandardTime_No_DST_Is_Successful()
        {
            var sourceDateTime = new DateTime(2018, 1, 1, 1, 0, 0);
            var timeZoneId = "Central Standard Time";
            var utcDate = _dateTimeConverter.ConvertToUtc(sourceDateTime, timeZoneId);

            Assert.Equal(new DateTime(2018, 1, 1, 7, 0, 0), utcDate);
        }

        [Fact]
        public void ConvertToUtc_From_CentralStandardTime_With_DST_Is_Successful()
        {
            var sourceDateTime = new DateTime(2018, 8, 1, 1, 0, 0);
            var timeZoneId = "Central Standard Time";
            var utcDate = _dateTimeConverter.ConvertToUtc(sourceDateTime, timeZoneId);

            Assert.Equal(new DateTime(2018, 8, 1, 6, 0, 0), utcDate);
        }

        [Fact]
        public void ConvertFromUtc_To_CentralStandardTime_No_DST_Is_Successful()
        {
            var sourceDateTime = new DateTime(2018, 1, 1, 7, 0, 0);
            var timeZoneId = "Central Standard Time";
            var centralDate = _dateTimeConverter.ConvertFromUtc(sourceDateTime, timeZoneId);

            Assert.Equal(new DateTime(2018, 1, 1, 1, 0, 0), centralDate);
        }

        [Fact]
        public void ConvertFromUtc_To_CentralStandardTime_With_DST_Is_Successful()
        {
            var sourceDateTime = new DateTime(2018, 8, 1, 6, 0, 0);
            var timeZoneId = "Central Standard Time";
            var centralDate = _dateTimeConverter.ConvertFromUtc(sourceDateTime, timeZoneId);

            Assert.Equal(new DateTime(2018, 8, 1, 1, 0, 0), centralDate);
        }
    }
}