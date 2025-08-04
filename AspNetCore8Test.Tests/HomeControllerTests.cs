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
        public void Error_ReturnsViewResultWithErrorViewModel(bool hasActivity)
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