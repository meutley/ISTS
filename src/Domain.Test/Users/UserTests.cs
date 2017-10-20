using System;

using Xunit;

using ISTS.Domain.Users;

namespace ISTS.Domain.Tests.Users
{
    public class UserTests
    {
        [Fact]
        public void Create_Returns_New_User()
        {
            var user = User.Create("myemail@company.com", "Person", "12345", "password1");

            Assert.NotNull(user);
            Assert.Equal("myemail@company.com", user.Email);
            Assert.Equal("Person", user.DisplayName);
            Assert.Equal("12345", user.PostalCode);
            Assert.NotNull(user.PasswordHash);
            Assert.NotNull(user.PasswordSalt);
        }

        [Fact]
        public void Create_Throws_ArgumentNullException_When_Password_Is_Null()
        {
            var ex = Assert.Throws<ArgumentNullException>(() => User.Create("myemail@company.com", "Person", "12345", null));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Create_Throws_ArgumentException_When_Password_Is_WhiteSpace()
        {
            var ex = Assert.Throws<ArgumentException>(() => User.Create("myemail@company.com", "Person", "12345", "    "));

            Assert.NotNull(ex);
        }
    }
}