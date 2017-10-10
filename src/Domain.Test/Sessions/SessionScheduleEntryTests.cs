using System;
using Xunit;

using ISTS.Domain.Sessions;
using ISTS.Domain.Studios;

namespace ISTS.Domain.Tests.Sessions
{
    public class SessionScheduleEntryTests
    {
        [Fact]
        public void Create_Throws_ArgumentNullException_When_Session_Is_Null()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => SessionScheduleEntry.Create(null, DateTime.Now, DateTime.Now));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Create_Returns_New_SessionScheduleEntry()
        {
            var startDateTime = DateTime.Now;
            var endDateTime = DateTime.Now.AddHours(2);

            var studio = Studio.Create();
            var session = Session.Create(studio);
            var sessionScheduleEntry = SessionScheduleEntry.Create(session, startDateTime, endDateTime);

            Assert.NotNull(sessionScheduleEntry);
            Assert.Equal(session.Id, sessionScheduleEntry.SessionId);
            Assert.Equal(startDateTime, sessionScheduleEntry.StartDateTime);
            Assert.Equal(endDateTime, sessionScheduleEntry.EndDateTime);
        }

        [Fact]
        public void Reschedule_Sets_StartDateTime_And_EndDateTime()
        {
            var startDateTime = DateTime.Now;
            var endDateTime = DateTime.Now.AddHours(2);

            var studio = Studio.Create();
            var session = Session.Create(studio);
            var sessionScheduleEntry = SessionScheduleEntry.Create(session, startDateTime, endDateTime);

            Assert.NotNull(sessionScheduleEntry);
            Assert.Equal(session.Id, sessionScheduleEntry.SessionId);
            Assert.Equal(startDateTime, sessionScheduleEntry.StartDateTime);
            Assert.Equal(endDateTime, sessionScheduleEntry.EndDateTime);

            startDateTime = startDateTime.AddDays(2);
            endDateTime = endDateTime.AddDays(2);

            sessionScheduleEntry.Reschedule(startDateTime, endDateTime);

            Assert.Equal(startDateTime, sessionScheduleEntry.StartDateTime);
            Assert.Equal(endDateTime, sessionScheduleEntry.EndDateTime);
        }
    }
}