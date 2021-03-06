using System;
using System.Threading.Tasks;

using Moq;
using Xunit;

using ISTS.Domain.Common;
using ISTS.Domain.PostalCodes;
using ISTS.Domain.Studios;

namespace ISTS.Domain.Test.Studios
{
    public class StudioValidatorTests
    {
        private readonly Mock<IPostalCodeValidator> _postalCodeValidator;
        private readonly Mock<IStudioRepository> _studioRepository;

        private readonly IStudioValidator _studioValidator;

        public StudioValidatorTests()
        {
            _postalCodeValidator = new Mock<IPostalCodeValidator>();
            _studioRepository = new Mock<IStudioRepository>();

            _studioValidator = new StudioValidator(_postalCodeValidator.Object, _studioRepository.Object);
        }

        [Fact]
        public async void ValidateAsync_Returns_Success_With_Alpha_Characters()
        {
            await _studioValidator.ValidateAsync(null, "StudioName", "FriendlyUrl", "12345");
        }

        [Fact]
        public async void ValidateAsync_Returns_Success_With_Alphanumeric_Characters_Hyphen_And_Underscore()
        {
            await _studioValidator.ValidateAsync(null, "StudioName", "Url-123_45", "12345");
        }

        [Fact]
        public async void ValidateAsync_Throws_ArgumentException_When_Url_Less_Than_Min_Length()
        {
            var ex = await Assert.ThrowsAsync<DomainValidationException>(() => _studioValidator.ValidateAsync(null, "StudioName", "A", "12345"));

            Assert.NotNull(ex);
            Assert.NotNull(ex.InnerException);
            Assert.IsType<ArgumentException>(ex.InnerException);
        }

        [Fact]
        public async void ValidateAsync_Throws_ArgumentException_When_Url_Longer_Than_Max_Length()
        {
            var ex = await Assert.ThrowsAsync<DomainValidationException>(
                () =>
                _studioValidator.ValidateAsync(null, "StudioName", "111111111111111111111111111111", "12345"));

            Assert.NotNull(ex);
            Assert.NotNull(ex.InnerException);
            Assert.IsType<ArgumentException>(ex.InnerException);
        }

        [Fact]
        public async void ValidateAsync_Throws_UriFormatException_When_Url_Contains_Invalid_Characters()
        {
            var ex = await Assert.ThrowsAsync<DomainValidationException>(
                () =>
                _studioValidator.ValidateAsync(null, "StudioName", "StudioUrl%^", "12345"));

            Assert.NotNull(ex);
            Assert.NotNull(ex.InnerException);
            Assert.IsType<UriFormatException>(ex.InnerException);
        }

        [Fact]
        public async void ValidateAsync_Throws_UriFormatException_When_Url_Does_Not_Start_With_Letter()
        {
            var ex = await Assert.ThrowsAsync<DomainValidationException>(
                () =>
                _studioValidator.ValidateAsync(null, "StudioName", "-InvalidUrl", "12345"));

            Assert.NotNull(ex);
            Assert.NotNull(ex.InnerException);
            Assert.IsType<UriFormatException>(ex.InnerException);
        }

        [Fact]
        public async void ValidateAsync_Returns_Success_When_Existing_Url_Belongs_To_Same_StudioId()
        {
            var studio = Studio.Create("StudioName", "FriendlyUrl", "12345", Guid.NewGuid(), _studioValidator);
            
            _studioRepository
                .Setup(r => r.GetByUrlAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(studio));

            await _studioValidator.ValidateAsync(studio.Id, "StudioName", "NewUrl", "12345");
        }

        [Fact]
        public async void ValidateAsync_Throws_StudioUrlInUseException_When_New_Studio()
        {
            var studio = Studio.Create("StudioName", "FriendlyUrl", "12345", Guid.NewGuid(), _studioValidator);

            _studioRepository
                .Setup(r => r.GetByUrlAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(studio));

            var ex = await Assert.ThrowsAsync<DomainValidationException>(
                () =>
                _studioValidator.ValidateAsync(null, "StudioName", "FriendlyUrl", "12345"));

            Assert.NotNull(ex);
            Assert.NotNull(ex.InnerException);
            Assert.IsType<StudioUrlInUseException>(ex.InnerException);
        }

        [Fact]
        public async void ValidateAsync_Throws_StudioUrlInUseException_When_Existing_Studio_Uses_Url()
        {
            var studio = Studio.Create("StudioName", "FriendlyUrl", "12345", Guid.NewGuid(), _studioValidator);

            _studioRepository
                .Setup(r => r.GetByUrlAsync(It.IsAny<string>()))
                .Returns(Task.FromResult(studio));

            var ex = await Assert.ThrowsAsync<DomainValidationException>(
                () =>
                _studioValidator.ValidateAsync(Guid.NewGuid(), "StudioName", "FriendlyUrl", "12345"));

            Assert.NotNull(ex);
            Assert.NotNull(ex.InnerException);
            Assert.IsType<StudioUrlInUseException>(ex.InnerException);
        }
    }
}