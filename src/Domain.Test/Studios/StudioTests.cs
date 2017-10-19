using System;
using System.Threading.Tasks;

using Moq;
using Xunit;

using ISTS.Domain.Studios;

namespace ISTS.Domain.Tests.Studios
{
    public class StudioTests
    {
        private readonly Mock<IStudioUrlValidator> _studioUrlValidator;

        public StudioTests()
        {
            _studioUrlValidator = new Mock<IStudioUrlValidator>();
        }
        
        [Fact]
        public void Create_Throws_ArgumentNullException_When_Name_Is_Null()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Studio.Create(null, "FriendlyUrl", _studioUrlValidator.Object));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Create_Throws_ArgumentNullException_When_FriendlyUrl_Is_Null()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Studio.Create("Name", null, _studioUrlValidator.Object));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Create_Returns_New_Studio()
        {
            _studioUrlValidator
                .Setup(v => v.ValidateAsync(It.IsAny<Guid?>(), It.IsAny<string>()))
                .Returns(Task.FromResult(StudioUrlValidatorResult.Success));
            
            var studio = Studio.Create("StudioName", "StudioFriendlyUrl", _studioUrlValidator.Object);

            Assert.NotNull(studio);
            Assert.Equal("StudioName", studio.Name);
            Assert.Equal("StudioFriendlyUrl", studio.FriendlyUrl);
        }

        [Fact]
        public void Create_Throws_ArgumentException()
        {
            _studioUrlValidator
                .Setup(v => v.ValidateAsync(It.IsAny<Guid?>(), It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException());

            var ex = Assert.Throws<ArgumentException>(() => Studio.Create("StudioName", "A", _studioUrlValidator.Object));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Create_Throws_UriFormatException()
        {
            _studioUrlValidator
                .Setup(v => v.ValidateAsync(It.IsAny<Guid?>(), It.IsAny<string>()))
                .ThrowsAsync(new UriFormatException());

            var ex = Assert.Throws<UriFormatException>(() => Studio.Create("StudioName", "%", _studioUrlValidator.Object));

            Assert.NotNull(ex);
        }

        [Fact]
        public void CreateRoom_Returns_New_StudioRoom_With_StudioId()
        {
            _studioUrlValidator
                .Setup(v => v.ValidateAsync(It.IsAny<Guid?>(), It.IsAny<string>()))
                .Returns(Task.FromResult(StudioUrlValidatorResult.Success));
            
            var studio = Studio.Create("StudioName", "StudioFriendlyUrl", _studioUrlValidator.Object);
            var room = studio.CreateRoom("RoomName");

            Assert.NotNull(room);
            Assert.Equal(studio.Id, room.StudioId);
            Assert.Equal("RoomName", room.Name);
        }
    }
}