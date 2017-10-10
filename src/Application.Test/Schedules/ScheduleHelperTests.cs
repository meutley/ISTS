using System;
using Xunit;

using ISTS.Application.Exceptions;
using ISTS.Application.Schedules;
using ISTS.Domain.Sessions;
using ISTS.Domain.Studios;

namespace ISTS.Application.Test
{
    public class ScheduleHelperTests
    {
        [Fact]
        public void Reschedule_Throws_InvalidDateRangeException_When_EndDate_Before_StartDate()
        {
            var startDateTime = DateTime.Now;
            var endDateTime = DateTime.Now.AddHours(2);

            var studio = Studio.Create();
            var session = Session.Create(studio);
            var sessionScheduleEntry = SessionScheduleEntry.Create(session, startDateTime, endDateTime);

            var ex = Assert.Throws<InvalidDateRangeException>(() => ScheduleHelper.Reschedule(sessionScheduleEntry, endDateTime, startDateTime));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Reschedule_Throws_InvalidDateRangeException_When_EndDate_Equal_StartDate()
        {
            var startDateTime = DateTime.Now;
            var endDateTime = DateTime.Now.AddHours(2);

            var studio = Studio.Create();
            var session = Session.Create(studio);
            var sessionScheduleEntry = SessionScheduleEntry.Create(session, startDateTime, endDateTime);

            var ex = Assert.Throws<InvalidDateRangeException>(() => ScheduleHelper.Reschedule(sessionScheduleEntry, startDateTime, startDateTime));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Reschedule_Sets_New_StartDateTime_And_EndDateTime()
        {
            var startDateTime = DateTime.Now;
            var endDateTime = DateTime.Now.AddHours(2);

            var studio = Studio.Create();
            var session = Session.Create(studio);
            var sessionScheduleEntry = SessionScheduleEntry.Create(session, startDateTime, endDateTime);

            Assert.Equal(startDateTime, sessionScheduleEntry.StartDateTime);
            Assert.Equal(endDateTime, sessionScheduleEntry.EndDateTime);
            
            startDateTime = startDateTime.AddDays(2);
            endDateTime = endDateTime.AddDays(2);

            ScheduleHelper.Reschedule(sessionScheduleEntry, startDateTime, endDateTime);

            Assert.Equal(sessionScheduleEntry.StartDateTime, startDateTime);
            Assert.Equal(sessionScheduleEntry.EndDateTime, endDateTime);
        }
    }
}