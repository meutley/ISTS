using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Moq;
using Xunit;

using ISTS.Api.Controllers;
using ISTS.Application.Studios;

namespace ISTS.Api.Test.Controllers
{
    public class StudiosControllerTests
    {
        private readonly Mock<IStudioService> _studioService;

        private readonly StudiosController _studiosController;

        public StudiosControllerTests()
        {
            _studioService = new Mock<IStudioService>();
            
            _studiosController = new StudiosController(_studioService.Object);
        }

        [Fact]
        public async void Get_Returns_NotFound()
        {
            _studioService
                .Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult<StudioDto>(null));

            var result = await _studiosController.Get(Guid.NewGuid());
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async void Get_Returns_StudioDto_When_Found()
        {
            var dto = new StudioDto
            {
                Id = Guid.NewGuid(),
                Name = "Studio",
                FriendlyUrl = "FriendlyUrl",
                PostalCode = "11111"
            };
            
            _studioService
                .Setup(s => s.GetAsync(It.IsAny<Guid>()))
                .Returns(Task.FromResult(dto));

            var result = await _studiosController.Get(dto.Id);
            Assert.IsType<OkObjectResult>(result);

            var okResult = result as OkObjectResult;
            Assert.IsType<StudioDto>(okResult.Value);

            var model = okResult.Value as StudioDto;
            Assert.Equal(dto.Id, model.Id);
            Assert.Equal(dto.Name, model.Name);
            Assert.Equal(dto.FriendlyUrl, model.FriendlyUrl);
            Assert.Equal(dto.PostalCode, model.PostalCode);
        }
    }
}