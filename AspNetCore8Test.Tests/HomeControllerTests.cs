using AspNetCore8Test.Controllers;
using AspNetCore8Test.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using System.Diagnostics;

namespace AspNetCore8Test.Tests
{
    public class HomeControllerTests
    {
        // 原本的 Fact 測試方式
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
            Assert.NotNull(result.Model); // 確保 Model 不是 null
            Assert.IsType<ErrorViewModel>(result.Model);
            var model = result.Model as ErrorViewModel;
            Assert.NotNull(model); // 確保 model 不是 null
            Assert.False(string.IsNullOrEmpty(model.RequestId));
        }

        // 新的 Theory 測試方式
        [Theory]
        [InlineData("Index")]
        [InlineData("Privacy")]
        public void ActionMethods_ReturnViewResult(string actionName)
        {
            // Arrange
            var logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(logger.Object);
            
            // Act
            IActionResult result = actionName switch
            {
                "Index" => controller.Index(),
                "Privacy" => controller.Privacy(),
                _ => throw new ArgumentException($"Unknown action: {actionName}")
            };

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Theory]
        [InlineData(true)]  // 當有 Activity.Current 時
        [InlineData(false)] // 當沒有 Activity.Current 時
        public void Error_ReturnsViewResultWithErrorViewModel_Theory(bool hasActivity)
        {
            // Arrange
            var logger = new Mock<ILogger<HomeController>>();
            var controller = new HomeController(logger.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            
            if (hasActivity)
            {
                // 模擬有 Activity
                var activity = new Activity("TestActivity");
                activity.Start();
            }
            else
            {
                // 確保沒有 Activity
                Activity.Current?.Stop();
            }

            // Act
            var result = controller.Error() as ViewResult;
            
            // Assert
            Assert.NotNull(result);
            Assert.IsType<ErrorViewModel>(result.Model);
            var model = result.Model as ErrorViewModel;
            Assert.NotNull(model);
            Assert.False(string.IsNullOrEmpty(model.RequestId));
            
            // 清理
            Activity.Current?.Stop();
        }
    }
}
