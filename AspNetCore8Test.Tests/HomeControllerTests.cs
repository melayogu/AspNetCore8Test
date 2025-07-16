using AspNetCore8Test.Controllers;
using AspNetCore8Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace AspNetCore8Test.Tests
{
    public class HomeControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult()
        {
            var logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(logger.Object);

            var result = controller.Index();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Privacy_ReturnsViewResult()
        {
            var logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(logger.Object);

            var result = controller.Privacy();

            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Error_ReturnsViewResultWithErrorViewModel()
        {
            var logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(logger.Object);

            // 設定 HttpContext，避免 NullReferenceException
            controller.ControllerContext.HttpContext = new DefaultHttpContext();

            var result = controller.Error() as ViewResult;

            Assert.NotNull(result);
            Assert.NotNull(result.Model); // 確保 Model 不為 null
            Assert.IsType<ErrorViewModel>(result.Model);
            var model = result.Model as ErrorViewModel;
            Assert.NotNull(model); // 確保 model 不為 null
            Assert.False(string.IsNullOrEmpty(model.RequestId));
        }
    }
}
