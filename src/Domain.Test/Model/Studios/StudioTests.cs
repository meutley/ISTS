using System;
using Xunit;

using ISTS.Domain.Model.Studios;

namespace ISTS.Domain.Tests.Model.Studios
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
    }
}