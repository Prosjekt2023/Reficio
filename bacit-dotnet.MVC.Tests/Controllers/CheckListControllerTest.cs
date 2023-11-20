using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Models.CheckList;
using bacit_dotnet.MVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace bacit_dotnet.MVC.Tests.Controllers
{
    public class CheckListControllerTests
    {
        [Fact]
        public void Index_GET_ReturnsViewResult()
        {
            // Arrange
            var mockRepository = new Mock<ICheckListRepository>();
            var controller = new CheckListController(mockRepository.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void Index_POST_WithValidModel_RedirectsToCheckListIndex()
        {
            // Arrange
            var mockRepository = new Mock<ICheckListRepository>();
            var controller = new CheckListController(mockRepository.Object);

            var validModel = new CheckListViewModel
            {
                // Set up valid model data for testing
            };

            // Act
            var result = controller.Index(validModel);

            // Assert
            Assert.IsType<RedirectToActionResult>(result);

            // You may want to check the specific controller and action names
            var redirectToAction = (RedirectToActionResult)result;
            Assert.Equal("Index", redirectToAction.ActionName);
            Assert.Equal("CheckList", redirectToAction.ControllerName);
        }

        [Fact]
        public void Index_POST_WithInvalidModel_ReturnsViewResultWithModel()
        {
            // Arrange
            var mockRepository = new Mock<ICheckListRepository>();
            var controller = new CheckListController(mockRepository.Object);

            var invalidModel = new CheckListViewModel
            {
                // Set up invalid model data for testing
            };

            // Add model state error to simulate invalid model state
            controller.ModelState.AddModelError("PropertyName", "Error message");

            // Act
            var result = controller.Index(invalidModel);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidModel, ((ViewResult)result).Model);
        }

        // Add more tests as needed for other controller actions
    }
}
