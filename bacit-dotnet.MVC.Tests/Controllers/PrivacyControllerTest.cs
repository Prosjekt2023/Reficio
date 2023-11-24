using bacit_dotnet.MVC.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

public class PrivacyControllerTests
{
    [Fact]
    public void Index_ReturnsViewResult()
    {
        // Arrange
        var controller = new PrivacyController();

        // Act
        var result = controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }
}