using System;
using Xunit;

using ISTS.Domain.Model.Sessions;
using ISTS.Domain.Model.Studios;

namespace ISTS.Domain.Tests.Model.Sessions
{
    public class SessionTests
    {
        private readonly Studio _studio;

        public SessionTests()
        {
            _studio = Studio.Create("Name", "FriendlyUrl");
        }

        [Fact]
        public void Create_Throws_ArgumentNullException_When_Studio_Is_Null()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Session.Create(null, null));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Create_Returns_New_Session()
        {
            var session = Session.Create(_studio, null);

            Assert.NotNull(session);
            Assert.Equal(session.StudioId, _studio.Id);
            Assert.Null(session.ScheduledTime);
        }
    }
}