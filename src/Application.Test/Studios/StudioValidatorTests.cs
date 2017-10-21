using System;
using System.Threading.Tasks;

using Moq;
using Xunit;

using ISTS.Application.Studios;
using ISTS.Domain.Studios;

namespace ISTS.Application.Test.Studios
{
    public class StudioValidatorTests
    {
        private readonly Mock<IStudioRepository> _studioRepository;

        private readonly IStudioValidator _studioValidator;

        public StudioValidatorTests()
        {
            _studioRepository = new Mock<IStudioRepository>();

            _studioValidator = new StudioValidator(_studioRepository.Object);
        }

        [Fact]
        public async void ValidateAsync_Returns_Success_With_Alpha_Characters()
        {
            var result = await _studioValidator.ValidateAsync(null, "StudioName", "FriendlyUrl");

            Assert.Equal(StudioValidatorResult.Success, result);
        }

        [Fact]
        public async void ValidateAsync_Returns_Success_With_Alphanumeric_Characters_Hyphen_And_Underscore()
        {
            var result = await _studioValidator.ValidateAsync(null, "StudioName", "Url-123_45");

            Assert.Equal(StudioValidatorResult.Success, result);
        }

        [Fact]
        public void ValidateAsync_Throws_ArgumentException_When_Url_Less_Than_Min_Length()
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _studioValidator.ValidateAsync(null, "StudioName", "A"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void ValidateAsync_Throws_ArgumentException_When_Url_Longer_Than_Max_Length()
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _studioValidator.ValidateAsync(null, "StudioName", "111111111111111111111111111111"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void ValidateAsync_Throws_UriFormatException_When_Url_Contains_Invalid_Characters()
        {
            var ex = Assert.ThrowsAsync<UriFormatException>(() => _studioValidator.ValidateAsync(null, "StudioName", "StudioUrl%^"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void ValidateAsync_Throws_UriFormatException_When_Url_Does_Not_Start_With_Letter()
        {
            var ex = Assert.ThrowsAsync<UriFormatException>(() => _studioValidator.ValidateAsync(null, "StudioName", "-InvalidUrl"));

            Assert.NotNull(ex);
        }

        [Fact]
        public async void ValidateAsync_Returns_Success_When_Existing_Url_Belongs_To_Same_StudioId()
        {
            var studio = Studio.Create("StudioName", "FriendlyUrl", Guid.NewGuid(), _studioValidator);
            
            _studioRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(studio));

            var result = await _studioValidator.ValidateAsync(studio.Id, "StudioName", "NewUrl");

            Assert.Equal(StudioValidatorResult.Success, result);
        }

        [Fact]
        public void ValidateAsync_Throws_StudioUrlInUseException_When_New_Studio()
        {
            var studio = Studio.Create("StudioName", "FriendlyUrl", Guid.NewGuid(), _studioValidator);

            _studioRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(studio));

            var ex = Assert.ThrowsAsync<StudioUrlInUseException>(() => _studioValidator.ValidateAsync(null, "StudioName", "FriendlyUrl"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void ValidateAsync_Throws_StudioUrlInUseException_When_Existing_Studio()
        {
            var studio = Studio.Create("StudioName", "FriendlyUrl", Guid.NewGuid(), _studioValidator);

            _studioRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(studio));

            var ex = Assert.ThrowsAsync<StudioUrlInUseException>(() => _studioValidator.ValidateAsync(Guid.NewGuid(), "StudioName", "FriendlyUrl"));

            Assert.NotNull(ex);
        }
    }
}