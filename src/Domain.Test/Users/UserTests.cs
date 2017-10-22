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

        public UserTests()
        {
            _userValidator = new Mock<IUserValidator>();
        }
        
        [Fact]
        public void Create_Returns_New_User()
        {
            var user = User.Create(_userValidator.Object, "myemail@company.com", "Person", "password1");

            Assert.NotNull(user);
            Assert.Equal("myemail@company.com", user.Email);
            Assert.Equal("Person", user.DisplayName);
            Assert.NotNull(user.PasswordHash);
            Assert.NotNull(user.PasswordSalt);
        }

        [Fact]
        public void Create_Throws_ArgumentNullException_When_Password_Is_Null()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => User.Create(_userValidator.Object, "myemail@company.com", "Person", null));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Create_Throws_ArgumentException_When_Password_Is_WhiteSpace()
        {
            var ex = Assert.Throws<ArgumentException>(() => User.Create(_userValidator.Object, "myemail@company.com", "Person", "    "));

            Assert.NotNull(ex);
        }
    }
}