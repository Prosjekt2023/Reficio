using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Models.Composite;
using bacit_dotnet.MVC.Repositories;

namespace bacit_dotnet.MVC.Tests.Controllers
{
    public class ServiceFormControllerTests
    {
        [Fact]
        public void Index_GET_ReturnsViewResult()
        {
            // Arrange
            var repositoryMock = new Mock<IServiceFormRepository>();
            var controller = new ServiceFormController(repositoryMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName);
        }

        [Fact]
        public void Index_POST_WithValidModel_RedirectsToServiceOrderIndex()
        {
            // Arrange
            var repositoryMock = new Mock<IServiceFormRepository>();
            var controller = new ServiceFormController(repositoryMock.Object);
            var validModel = new ServiceFormViewModel { /* Set valid properties */ };

            // Act
            var result = controller.Index(validModel);

            // Assert
            var redirectToActionResult = Assert.IsType<RedirectToActionResult>(result);
            Assert.Equal("Index", redirectToActionResult.ActionName);
            Assert.Equal("ServiceOrder", redirectToActionResult.ControllerName);
        }

        [Fact]
        public void Index_POST_WithInvalidModel_ReturnsViewResult()
        {
            // Arrange
            var repositoryMock = new Mock<IServiceFormRepository>();
            var controller = new ServiceFormController(repositoryMock.Object);
            var invalidModel = new ServiceFormViewModel { /* Set invalid properties for triggering model state errors */ };
            controller.ModelState.AddModelError("PropertyName", "Error message");

            // Act
            var result = controller.Index(invalidModel);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidModel, viewResult.Model);
        }

        [Fact]
        public void Index_POST_WithValidModel_CallsRepositoryInsert()
        {
            // Arrange
            var repositoryMock = new Mock<IServiceFormRepository>();
            var controller = new ServiceFormController(repositoryMock.Object);
            var validModel = new ServiceFormViewModel { /* Set valid properties */ };

            // Act
            var result = controller.Index(validModel);

            // Assert
            repositoryMock.Verify(repo => repo.Insert(validModel), Times.Once);
        }

        [Fact]
        public void Index_POST_WithInvalidModel_DoesNotCallRepositoryInsert()
        {
            // Arrange
            var repositoryMock = new Mock<IServiceFormRepository>();
            var controller = new ServiceFormController(repositoryMock.Object);
            var invalidModel = new ServiceFormViewModel { /* Set invalid properties for triggering model state errors */ };
            controller.ModelState.AddModelError("PropertyName", "Error message");

            // Act
            var result = controller.Index(invalidModel);

            // Assert
            repositoryMock.Verify(repo => repo.Insert(It.IsAny<ServiceFormViewModel>()), Times.Never);
        }
    }
}
