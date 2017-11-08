using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

using AutoMapper;
using Moq;
using Xunit;

using ISTS.Application.Users;
using ISTS.Domain.Users;

namespace ISTS.Application.Test.Users
{
    public class UserServiceTests
    {
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IUserValidator> _userValidator;
        private readonly Mock<IUserRepository> _userRepository;
        private readonly Mock<IUserPasswordService> _userPasswordService;

        private readonly IUserService _userService;

        public UserServiceTests()
        {
            _mapper = new Mock<IMapper>();
            _userValidator = new Mock<IUserValidator>();
            _userRepository = new Mock<IUserRepository>();
            _userPasswordService = new Mock<IUserPasswordService>();

            _userService = new UserService(
                _mapper.Object,
                _userValidator.Object,
                _userRepository.Object,
                _userPasswordService.Object);

            _mapper
                .Setup(x => x.Map<UserDto>(It.IsAny<User>()))
                .Returns((User source) =>
                {
                    return new UserDto
                    {
                        Id = source.Id,
                        Email = source.Email,
                        DisplayName = source.DisplayName,
                        PostalCode = source.PostalCode
                    };
                });
        }

        [Theory]
        [InlineData(null, "Password")]
        [InlineData("Email", null)]
        [InlineData("", "Password")]
        [InlineData("Email", "")]
        public async void AuthenticateAsync_Returns_Null_If_Email_Password_Null_Or_Empty(
            string email,
            string password)
        {
            var result = await _userService.AuthenticateAsync(email, password);
            Assert.Null(result);
        }

        [Fact]
        public async void AuthenticateAsync_Returns_Null_If_User_Not_Found()
        {
            var users = Enumerable.Empty<User>().AsQueryable();
            
            _userRepository
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns((Expression<Func<User, bool>> predicate) => Task.FromResult(users.Where(predicate).ToList()));
            
            var result = await _userService.AuthenticateAsync("my@email.com", "Password1");
            Assert.Null(result);
        }

        [Fact]
        public async void AuthenticateAsync_Returns_Null_If_Password_Does_Not_Match()
        {
            var email = "my@email.com";
            var user = User.Create(
                _userValidator.Object,
                _userPasswordService.Object,
                email,
                "DisplayName",
                "BadPassword",
                "11111",
                1);

            var users = new List<User>
            {
                user
            }.AsQueryable();
            
            _userRepository
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns((Expression<Func<User, bool>> predicate) => Task.FromResult(users.Where(predicate).ToList()));

            _userPasswordService
                .Setup(v => v.ValidateHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(false);
            
            var result = await _userService.AuthenticateAsync("my@email.com", "Password1");
            Assert.Null(result);
        }

        [Fact]
        public async void AuthenticateAsync_Returns_UserDto_If_Valid()
        {
            var email = "my@email.com";
            var user = User.Create(
                _userValidator.Object,
                _userPasswordService.Object,
                email,
                "DisplayName",
                "Password1",
                "11111",
                1);

            var users = new List<User>
            {
                user
            }.AsQueryable();
            
            _userRepository
                .Setup(r => r.GetAsync(It.IsAny<Expression<Func<User, bool>>>()))
                .Returns((Expression<Func<User, bool>> predicate) => Task.FromResult(users.Where(predicate).ToList()));

            _userPasswordService
                .Setup(v => v.ValidateHash(It.IsAny<string>(), It.IsAny<byte[]>(), It.IsAny<byte[]>()))
                .Returns(true);
            
            var result = await _userService.AuthenticateAsync("my@email.com", "Password1");

            Assert.NotNull(result);
            Assert.Equal(user.Id, result.Id);
            Assert.Equal(user.DisplayName, result.DisplayName);
            Assert.Equal(user.PostalCode, result.PostalCode);
        }

        [Fact]
        public async void CreateAsync_Returns_UserDto()
        {
            var userEntity = User.Create(
                _userValidator.Object,
                _userPasswordService.Object,
                "my@email.com",
                "DisplayName",
                "Password1",
                "11111",
                1);

            var dto = new UserPasswordDto
            {
                Id = userEntity.Id,
                Email = userEntity.Email,
                DisplayName = userEntity.DisplayName,
                PostalCode = userEntity.PostalCode,
                Password = "Password1"
            };

            _userRepository
                .Setup(r => r.CreateAsync(It.IsAny<User>()))
                .Returns(Task.FromResult(userEntity));

            var result = await _userService.CreateAsync(dto);

            Assert.NotNull(result);
            Assert.Equal(dto.Id, result.Id);
            Assert.Equal(dto.Email, result.Email);
            Assert.Equal(dto.DisplayName, result.DisplayName);
            Assert.Equal(dto.PostalCode, result.PostalCode);
        }
    }
}