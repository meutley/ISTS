using System;
using Xunit;

using Moq;

using ISTS.Domain.Studios;

namespace ISTS.Domain.Tests.Studios
{
    public class StudioTests
    {
        private static readonly Studio _studio = Studio.Create("StudioName", "StudioFriendlyUrl");

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
        public void CreateRoom_Returns_New_StudioRoom_With_StudioId()
        {
            var room = _studio.CreateRoom("RoomName");

            Assert.NotNull(room);
            Assert.Equal(_studio.Id, room.StudioId);
            Assert.Equal("RoomName", room.Name);
        }
    }
}