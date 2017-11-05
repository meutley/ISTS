using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

using Moq;
using Xunit;

using ISTS.Api.Controllers;
using ISTS.Api.Helpers;
using ISTS.Application.Users;

namespace ISTS.Api.Test.Controllers
{
    public class UsersControllerTests
    {
        private Mock<IOptions<ApplicationSettings>> _options;
        private Mock<IUserService> _userService;

        private UsersController _usersController;

        public UsersControllerTests()
        {
            _options = new Mock<IOptions<ApplicationSettings>>();
            _userService = new Mock<IUserService>();

            _usersController = new UsersController(_options.Object, _userService.Object);
        }

        [Fact]
        public async void Register_Returns_OkObjectResult_With_UserDto()
        {
            var dto = new UserPasswordDto
            {
                Email = "my@email.com",
                DisplayName = "My User",
                PostalCode = "11111"
            };

            var expectedModel = new UserDto
            {
                Id = Guid.NewGuid(),
                Email = dto.Email,
                DisplayName = dto.DisplayName,
                PostalCode = dto.PostalCode
            };

            _userService
                .Setup(s => s.CreateAsync(It.IsAny<UserPasswordDto>()))
                .Returns(Task.FromResult(expectedModel));

            var result = await _usersController.Register(dto);
            Assert.IsType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsType<UserDto>(okResult.Value);

            var model = okResult.Value as UserDto;
            Assert.Equal(expectedModel.Id, model.Id);
            Assert.Equal(expectedModel.Email, model.Email);
            Assert.Equal(expectedModel.DisplayName, model.DisplayName);
            Assert.Equal(expectedModel.PostalCode, model.PostalCode);
        }

        [Fact]
        public async void Authenticate_Returns_UnauthorizedResult()
        {
            var dto = new UserPasswordDto
            {
                Email = "my@email.com",
                Password = "BadP@ssw0rd"
            };

            _userService
                .Setup(s => s.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>()))
                .Returns(Task.FromResult<UserDto>(null));

            var result = await _usersController.Authenticate(dto);
            Assert.IsType<UnauthorizedResult>(result);
        }
    }
}