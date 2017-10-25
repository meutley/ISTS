using System;

using Moq;
using Xunit;

using ISTS.Domain.Common;
using ISTS.Domain.Users;

namespace ISTS.Domain.Tests.Users
{
    public class UserTests
    {
        private readonly Mock<IUserValidator> _userValidator;
        private readonly Mock<IUserPasswordService> _userPasswordService;

        public UserTests()
        {
            _userValidator = new Mock<IUserValidator>();
            _userPasswordService = new Mock<IUserPasswordService>();
        }
        
        [Fact]
        public void Create_Returns_New_User()
        {
            byte[] passwordHash = {};
            byte[] passwordSalt = {};
            _userPasswordService
                .Setup(s => s.CreateHash(It.IsAny<string>(), out passwordHash, out passwordSalt));

            var user = User.Create(
                _userValidator.Object,
                _userPasswordService.Object,
                "myemail@company.com",
                "Person",
                "password1",
                "00000");

            Assert.NotNull(user);
            Assert.Equal("myemail@company.com", user.Email);
            Assert.Equal("Person", user.DisplayName);
            Assert.Equal(passwordHash, user.PasswordHash);
            Assert.Equal(passwordSalt, user.PasswordSalt);
        }

        [Fact]
        public void Create_Throws_ArgumentNullException_When_Password_Is_Null()
        {
            _userValidator
                .Setup(
                    v =>
                    v.ValidateAsync(
                        It.IsAny<Guid?>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .ThrowsAsync(new ArgumentNullException());
            
            var ex = Assert.Throws<ArgumentNullException>(
                () =>
                    User.Create(_userValidator.Object, _userPasswordService.Object, "myemail@company.com", "Person", null, "00000"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Create_Throws_ArgumentException_When_Password_Is_WhiteSpace()
        {
            _userValidator
                .Setup(
                    v =>
                    v.ValidateAsync(
                        It.IsAny<Guid?>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>(),
                        It.IsAny<string>()))
                .ThrowsAsync(new ArgumentException());
            
            var ex = Assert.Throws<ArgumentException>(
                () =>
                    User.Create(_userValidator.Object, _userPasswordService.Object, "myemail@company.com", "Person", "    ", "00000"));

            Assert.NotNull(ex);
        }
    }
}