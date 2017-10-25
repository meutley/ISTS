using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using Moq;
using Xunit;

using ISTS.Application.Users;
using ISTS.Domain.Common;
using ISTS.Domain.PostalCodes;
using ISTS.Domain.Users;

namespace ISTS.Application.Test.Users
{
    public class UserValidatorTests
    {
        private readonly Mock<IEmailValidator> _emailValidator;
        private readonly Mock<IPostalCodeValidator> _postalCodeValidator;
        private readonly Mock<IUserRepository> _userRepository;

        private readonly IUserValidator _userValidator;

        public UserValidatorTests()
        {
            _emailValidator = new Mock<IEmailValidator>();
            _postalCodeValidator = new Mock<IPostalCodeValidator>();
            _userRepository = new Mock<IUserRepository>();

            _userRepository
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns(Task.FromResult(new List<User>()));

            _userValidator = new UserValidator(
                _emailValidator.Object,
                _postalCodeValidator.Object,
                _userRepository.Object);
        }

        [Fact]
        public void Validate_Is_Successful()
        {
            _userValidator.Validate(Guid.NewGuid(), "email@company.com", "DisplayName", "Password", "00000");
        }

        [Fact]
        public void Validate_Throws_FormatException_When_Email_Format_Is_Invalid()
        {
            _emailValidator
                .Setup(v => v.Validate(It.IsAny<string>()))
                .Throws<FormatException>();

            var ex = Assert.Throws<FormatException>(
                () =>
                    _userValidator.Validate(Guid.NewGuid(), "bad.email", "DisplayName", "Password", "00000"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Validate_Throws_PostalCodeFormatException_When_PostalCode_Format_Invalid()
        {
            _postalCodeValidator
                .Setup(v => v.ValidateAsync(It.IsAny<string>(), It.IsAny<PostalCodeValidatorTypes>()))
                .Throws<PostalCodeFormatException>();

            var ex = Assert.Throws<PostalCodeFormatException>(
                () =>
                    _userValidator.Validate(Guid.NewGuid(), "a@b.com", "DisplayName", "Password", "abcd"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Validate_Throws_PostalCodeNotFoundException()
        {
            _postalCodeValidator
                .Setup(v => v.ValidateAsync(It.IsAny<string>(), It.IsAny<PostalCodeValidatorTypes>()))
                .Throws<PostalCodeNotFoundException>();

            var ex = Assert.Throws<PostalCodeNotFoundException>(
                () =>
                    _userValidator.Validate(Guid.NewGuid(), "a@b.com", "DisplayName", "Password", "00000"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Validate_Throws_EmailInUseException()
        {
            var existingUsers = new List<User>
            {
                User.Create(
                    _userValidator,
                    "existing@email.com",
                    "DisplayName",
                    "Password",
                    "12345")
            }.AsQueryable();

            _userRepository
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns((Expression<Func<User, bool>> predicate) => Task.FromResult(existingUsers.Where(predicate).ToList()));

            var ex = Assert.Throws<EmailInUseException>(
                () =>
                    _userValidator.Validate(Guid.NewGuid(), "existing@email.com", "DisplayName", "Password", "00000"));

            Assert.NotNull(ex);
        }

        [Fact]
        public void Validate_Is_Successful_When_Email_Not_In_Use()
        {
            var existingUsers = new List<User>
            {
                User.Create(
                    _userValidator,
                    "existing@email.com",
                    "DisplayName",
                    "Password",
                    "12345")
            }.AsQueryable();

            _userRepository
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns((Expression<Func<User, bool>> predicate) => Task.FromResult(existingUsers.Where(predicate).ToList()));

            _userValidator.Validate(Guid.NewGuid(), "new@email.com", "DisplayName", "Password", "00000");
        }
    }
}