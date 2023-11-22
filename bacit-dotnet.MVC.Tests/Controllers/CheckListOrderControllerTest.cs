using Microsoft.AspNetCore.Mvc;
using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Models.Composite;
using bacit_dotnet.MVC.Repositories;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace bacit_dotnet.MVC.Tests.Controllers
{
    public class CheckListOrderControllerTests
    {
        [Fact]
        public void Index_ReturnsViewWithModel()
        {
            // Arrange
            var repositoryMock = new Mock<ICheckListRepository>();
            repositoryMock.Setup(repo => repo.GetSomeOrderInfo())
                .Returns(new List<CheckListViewModel>());  // Return an empty list for simplicity

            var controller = new CheckListOrderController(repositoryMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            Assert.IsType<List<CheckListViewModel>>(viewResult.Model);
        }

        [Fact]
        public void Index_ReturnsNotFoundResult_WhenOrderListIsNull()
        {
            // Arrange
            var repositoryMock = new Mock<ICheckListRepository>();
            repositoryMock.Setup(repo => repo.GetSomeOrderInfo())
                .Returns((List<CheckListViewModel>)null);

            var controller = new CheckListOrderController(repositoryMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void Index_HandlesError_ReturnsErrorView()
        {
            // Arrange
            var repositoryMock = new Mock<ICheckListRepository>();
            repositoryMock.Setup(repo => repo.GetSomeOrderInfo())
                .Throws(new Exception("Simulated error"));

            var controller = new CheckListOrderController(repositoryMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Equal("Error", viewResult.ViewName);  // Assuming you have an "Error" view
        }
    }
}
