using System;
using System.Threading.Tasks;

using Moq;
using Xunit;

using ISTS.Application.Studios;
using ISTS.Domain.Exceptions;
using ISTS.Domain.Studios;

namespace ISTS.Application.Test.Studios
{
    public class StudioUrlValidatorTests
    {
        private readonly Mock<IStudioRepository> _studioRepository;

        private readonly IStudioUrlValidator _studioUrlValidator;

        public StudioUrlValidatorTests()
        {
            _studioRepository = new Mock<IStudioRepository>();

            _studioUrlValidator = new StudioUrlValidator(_studioRepository.Object);
        }

        [Fact]
        public async void ValidateAsync_Returns_Success_With_Alpha_Characters()
        {
            var result = await _studioUrlValidator.ValidateAsync(null, "FriendlyUrl");

            Assert.Equal(StudioUrlValidatorResult.Success, result);
        }

        [Fact]
        public async void ValidateAsync_Returns_Success_With_Alphanumeric_Characters_Hyphen_And_Underscore()
        {
            var result = await _studioUrlValidator.ValidateAsync(null, "Url-123_45");

            Assert.Equal(StudioUrlValidatorResult.Success, result);
        }

        [Fact]
        public void ValidateAsync_Throws_ArgumentException_When_Url_Less_Than_Min_Length()
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _studioUrlValidator.ValidateAsync(null, "A"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void ValidateAsync_Throws_ArgumentException_When_Url_Longer_Than_Max_Length()
        {
            var ex = Assert.ThrowsAsync<ArgumentException>(() => _studioUrlValidator.ValidateAsync(null, "111111111111111111111111111111"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void ValidateAsync_Throws_UriFormatException_When_Url_Contains_Invalid_Characters()
        {
            var ex = Assert.ThrowsAsync<UriFormatException>(() => _studioUrlValidator.ValidateAsync(null, "StudioUrl%^"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void ValidateAsync_Throws_UriFormatException_When_Url_Does_Not_Start_With_Letter()
        {
            var ex = Assert.ThrowsAsync<UriFormatException>(() => _studioUrlValidator.ValidateAsync(null, "-InvalidUrl"));

            Assert.NotNull(ex);
        }

        [Fact]
        public async void ValidateAsync_Returns_Success_When_Existing_Url_Belongs_To_Same_StudioId()
        {
            var studio = Studio.Create("StudioName", "FriendlyUrl", _studioUrlValidator);
            
            _studioRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(studio));

            var result = await _studioUrlValidator.ValidateAsync(studio.Id, "NewUrl");

            Assert.Equal(StudioUrlValidatorResult.Success, result);
        }

        [Fact]
        public void ValidateAsync_Throws_StudioUrlInUseException_When_New_Studio()
        {
            var studio = Studio.Create("StudioName", "FriendlyUrl", _studioUrlValidator);

            _studioRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(studio));

            var ex = Assert.ThrowsAsync<StudioUrlInUseException>(() => _studioUrlValidator.ValidateAsync(null, "FriendlyUrl"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void ValidateAsync_Throws_StudioUrlInUseException_When_Existing_Studio()
        {
            var studio = Studio.Create("StudioName", "FriendlyUrl", _studioUrlValidator);

            _studioRepository
                .Setup(r => r.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(studio));

            var ex = Assert.ThrowsAsync<StudioUrlInUseException>(() => _studioUrlValidator.ValidateAsync(Guid.NewGuid(), "FriendlyUrl"));

            Assert.NotNull(ex);
        }
    }
}