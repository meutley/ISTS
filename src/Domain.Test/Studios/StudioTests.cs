using System;
using System.Threading.Tasks;

using Moq;
using Xunit;

using ISTS.Domain.Studios;

namespace ISTS.Domain.Tests.Studios
{
    public class StudioTests
    {
        private readonly Mock<IStudioValidator> _studioValidator;

        public StudioTests()
        {
            _studioValidator = new Mock<IStudioValidator>();
        }
        
        [Fact]
        public void Create_Throws_ArgumentNullException_When_Name_Is_Null()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Studio.Create(null, "FriendlyUrl", _studioValidator.Object));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Create_Throws_ArgumentNullException_When_FriendlyUrl_Is_Null()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => Studio.Create("Name", null, _studioValidator.Object));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Create_Returns_New_Studio()
        {
            _studioValidator
                .Setup(v => v.ValidateAsync(It.IsAny<Guid?>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(StudioValidatorResult.Success));
            
            var studio = Studio.Create("StudioName", "StudioFriendlyUrl", _studioValidator.Object);

            Assert.NotNull(studio);
            Assert.Equal("StudioName", studio.Name);
            Assert.Equal("StudioFriendlyUrl", studio.FriendlyUrl);
        }

        [Fact]
        public void Create_Throws_ArgumentException()
        {
            _studioValidator
                .Setup(v => v.ValidateAsync(It.IsAny<Guid?>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException());

            var ex = Assert.Throws<ArgumentException>(() => Studio.Create("StudioName", "A", _studioValidator.Object));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Create_Throws_UriFormatException()
        {
            _studioValidator
                .Setup(v => v.ValidateAsync(It.IsAny<Guid?>(), It.IsAny<string>(), It.IsAny<string>()))
                .ThrowsAsync(new UriFormatException());

            var ex = Assert.Throws<UriFormatException>(() => Studio.Create("StudioName", "%", _studioValidator.Object));

            Assert.NotNull(ex);
        }

        [Fact]
        public void CreateRoom_Returns_New_StudioRoom_With_StudioId()
        {
            _studioValidator
                .Setup(v => v.ValidateAsync(It.IsAny<Guid?>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult(StudioValidatorResult.Success));
            
            var studio = Studio.Create("StudioName", "StudioFriendlyUrl", _studioValidator.Object);
            var room = studio.CreateRoom("RoomName");

            Assert.NotNull(room);
            Assert.Equal(studio.Id, room.StudioId);
            Assert.Equal("RoomName", room.Name);
        }
    }
}