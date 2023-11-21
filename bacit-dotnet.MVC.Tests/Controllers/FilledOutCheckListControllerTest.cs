using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Models.Composite;
using bacit_dotnet.MVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace bacit_dotnet.MVC.Tests.Controllers
{
    public class FilledOutCheckListControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult_WhenCheckListExists()
        {
            // Arrange
            var mockRepository = new Mock<ICheckListRepository>();
            mockRepository.Setup(repo => repo.GetOneRowById(It.IsAny<int>()))
                .Returns<int>(id => new CheckListViewModel
                {
                    // Legg til dine faktiske egenskaper her
                });

            var controller = new FilledOutCheckListController(mockRepository.Object);

            // Act
            var result = controller.Index(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CheckListViewModel>(viewResult.Model);
            Assert.NotNull(model);
        }

        [Fact]
        public void Index_ReturnsNotFoundResult_WhenCheckListDoesNotExist()
        {
            // Arrange
            var mockRepository = new Mock<ICheckListRepository>();
            mockRepository.Setup(repo => repo.GetOneRowById(It.IsAny<int>()))
                .Returns((CheckListViewModel)null);

            var controller = new FilledOutCheckListController(mockRepository.Object);

            // Act
            var result = controller.Index(1);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }
    }
}