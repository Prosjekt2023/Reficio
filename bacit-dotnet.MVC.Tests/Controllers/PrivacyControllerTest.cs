using bacit_dotnet.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace bacit_dotnet.MVC.Tests.Controllers
{
    public class PrivacyControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var controller = new PrivacyController();

            // Act
            var result = controller.Index() as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}