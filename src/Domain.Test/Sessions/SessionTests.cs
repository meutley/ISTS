using System;
using Xunit;

using ISTS.Domain.Sessions;
using ISTS.Domain.Studios;

namespace ISTS.Domain.Tests.Sessions
{
    public class SessionTests
    {
        [Fact]
        public void Create_Throws_ArgumentNullException_When_Studio_Is_Null()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Session.Create(null));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Create_Returns_New_Session()
        {
            var studio = Studio.Create();
            var session = Session.Create(studio);

            Assert.NotNull(session);
            Assert.Equal(session.StudioId, studio.Id);
        }
    }
}