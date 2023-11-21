using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Models.Composite;
using bacit_dotnet.MVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace bacit_dotnet.MVC.Tests.Controllers
{
    public class FilledOutServiceFormControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult_WhenServiceFormExists()
        {
            // Arrange
            var mockRepository = new Mock<IServiceFormRepository>();
            mockRepository.Setup(repo => repo.GetOneRowById(It.IsAny<int>()))
                .Returns<int>(id => new ServiceFormViewModel());

            var controller = new FilledOutServiceFormController(mockRepository.Object);

            // Act
            var result = controller.Index(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ServiceFormViewModel>(viewResult.Model);
            Assert.NotNull(model);
        }

        [Fact]
        public void Index_ReturnsNotFoundResult_WhenServiceFormDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<IServiceFormRepository>();
            mockRepository.Setup(repo => repo.GetOneRowById(It.IsAny<int>()))
                .Returns((ServiceFormViewModel)null);

            var controller = new FilledOutServiceFormController(mockRepository.Object);

            // Act
            var result = controller.Index(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Index_HandlesInvalidId_ReturnsNotFoundResult()
        {
            // Arrange
            var mockRepository = new Mock<IServiceFormRepository>();
            mockRepository.Setup(repo => repo.GetOneRowById(It.IsAny<int>()))
                .Returns((ServiceFormViewModel)null);

            var controller = new FilledOutServiceFormController(mockRepository.Object);

            // Act
            var result = controller.Index(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Index_ReturnsCorrectViewModelType()
        {
            // Arrange
            var mockRepository = new Mock<IServiceFormRepository>();
            mockRepository.Setup(repo => repo.GetOneRowById(It.IsAny<int>()))
                .Returns<int>(id => new ServiceFormViewModel());

            var controller = new FilledOutServiceFormController(mockRepository.Object);

            // Act
            var result = controller.Index(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ServiceFormViewModel>(result.ViewData.Model);
        }

        [Fact]
        public void Index_ReturnsErrorView_WhenExceptionThrown()
        {
            // Arrange
            var mockRepository = new Mock<IServiceFormRepository>();
            mockRepository.Setup(repo => repo.GetOneRowById(It.IsAny<int>()))
                .Throws(new Exception("Simulated error"));

            var controller = new FilledOutServiceFormController(mockRepository.Object);

            // Act
            var result = controller.Index(1) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Error", result.ViewName);
        }
    }
}
