using System;
using Xunit;

using ISTS.Domain.Schedules;
using ISTS.Domain.Studios;

namespace ISTS.Domain.Tests.Studios
{
    public class StudioTests
    {
        [Fact]
        public void Create_Throws_ArgumentNullException_When_Name_Is_Null()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Studio.Create(null, "FriendlyUrl"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Create_Throws_ArgumentNullException_When_FriendlyUrl_Is_Null()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Studio.Create("Name", null));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Create_Returns_New_Studio()
        {
            var studio = Studio.Create("StudioName", "StudioFriendlyUrl");

            Assert.NotNull(studio);
            Assert.Equal("StudioName", studio.Name);
            Assert.Equal("StudioFriendlyUrl", studio.FriendlyUrl);
        }

        [Fact]
        public void CreateSession_Returns_New_StudioSession_With_StudioId_And_No_Schedule()
        {
            var studio = Studio.Create("StudioName", "StudioFriendlyUrl");

            Assert.NotNull(studio);
            
            var session = studio.CreateSession(null);

            Assert.NotNull(session);
            Assert.Equal(studio.Id, session.StudioId);
            Assert.Null(session.ScheduledTime);
        }

        [Fact]
        public void CreateSession_Returns_New_StudioSession_With_StudioId_And_Schedule()
        {
            var start = DateTime.Now;
            var end = start.AddHours(2);

            var schedule = DateRange.Create(start, end);

            var studio = Studio.Create("StudioName", "StudioFriendlyUrl");

            Assert.NotNull(studio);
            
            var session = studio.CreateSession(schedule);

            Assert.NotNull(session);
            Assert.Equal(studio.Id, session.StudioId);
            Assert.NotNull(session.ScheduledTime);
            Assert.Equal(start, session.ScheduledTime.Start);
            Assert.Equal(end, session.ScheduledTime.End);
        }
    }
}