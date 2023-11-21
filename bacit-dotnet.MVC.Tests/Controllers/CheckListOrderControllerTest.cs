using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace bacit_dotnet.MVC.Tests.Controllers
{
    public class CheckListOrderControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var mockRepository = new Mock<ICheckListRepository>();
            var controller = new CheckListOrderController(mockRepository.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }
        
    }
}