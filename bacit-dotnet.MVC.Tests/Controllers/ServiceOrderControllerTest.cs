using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class ServiceOrderControllerTests
{
    [Fact]
    public void Index_ReturnsViewWithModel()
    {
        // Arrange
        var mockRepository = new Mock<IServiceFormRepository>();
        var controller = new ServiceOrderController(mockRepository.Object);
        var serviceFormEntry = new ServiceFormEntry(); // Erstatt med reell klasse og data

        mockRepository.Setup(repo => repo.GetSomeOrderInfo()).Returns(serviceFormEntry);

        // Act
        var result = controller.Index() as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(serviceFormEntry, result.Model);
    }

    [Fact]
    public void Index_ReturnsUnauthorized_WhenNotAuthenticated()
    {
        // Arrange
        var controller = new ServiceOrderController(Mock.Of<IServiceFormRepository>());
        controller.ControllerContext = new ControllerContext();
        // Simulerer ikke-autentisert tilstand
        controller.ControllerContext.HttpContext = new DefaultHttpContext { User = new ClaimsPrincipal() };

        // Act
        var result = controller.Index() as UnauthorizedResult;

        // Assert
        Assert.NotNull(result);
    }
}