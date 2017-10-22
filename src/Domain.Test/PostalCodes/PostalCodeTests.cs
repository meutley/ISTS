using System;

using Moq;
using Xunit;

using ISTS.Domain.PostalCodes;

namespace ISTS.Domain.Tests.PostalCodes
{
    public class PostalCodeTests
    {
        private readonly Mock<IPostalCodeValidator> _postalCodeValidator;

        public PostalCodeTests()
        {
            _postalCodeValidator = new Mock<IPostalCodeValidator>();
        }
        
        [Fact]
        public void Create_Returns_New_PostalCode()
        {
            var result = PostalCode.Create(_postalCodeValidator.Object, "12345", "Smalltown", "US", 1.0m, 1.0m);

            Assert.NotNull(result);
            Assert.Equal("12345", result.Code);
        }
    }
}