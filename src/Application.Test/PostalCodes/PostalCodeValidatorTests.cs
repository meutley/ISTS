using System;

using Moq;
using Xunit;

using ISTS.Application.PostalCodes;
using ISTS.Domain.PostalCodes;

namespace ISTS.Application.Test.PostalCodes
{
    public class PostalCodeValidatorTests
    {
        private readonly Mock<IPostalCodeRepository> _postalCodeRepository;
        private readonly IPostalCodeValidator _postalCodeValidator;

        public PostalCodeValidatorTests()
        {
            _postalCodeRepository = new Mock<IPostalCodeRepository>();

            _postalCodeValidator = new PostalCodeValidator(_postalCodeRepository.Object);
        }

        [Fact]
        public async void ValidateAsync_Is_Successful_When_PostalCode_Is_Null()
        {
            string postalCode = null;
            await _postalCodeValidator.ValidateAsync(postalCode);
        }

        [Fact]
        public async void ValidateAsync_Is_Successful_When_FiveDigit_Code()
        {
            string postalCode = "12345";
            await _postalCodeValidator.ValidateAsync(postalCode);
        }

        [Fact]
        public void ValidateAsync_Throws_PostalCodeFormatException_When_Code_Contains_Letters()
        {
            string postalCode = "A1234";
            var ex = Assert.ThrowsAsync<PostalCodeFormatException>(() => _postalCodeValidator.ValidateAsync(postalCode));

            Assert.NotNull(ex);
        }

        [Fact]
        public void ValidateAsync_Throws_PostalCodeFormatException_When_Code_Format_Invalid()
        {
            string postalCode = "12";
            var ex = Assert.ThrowsAsync<PostalCodeFormatException>(() => _postalCodeValidator.ValidateAsync(postalCode));

            Assert.NotNull(ex);
        }
    }
}