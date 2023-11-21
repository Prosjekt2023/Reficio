using System;
using System.Collections.Generic;
using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Models.Composite;
using bacit_dotnet.MVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
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
                .Returns(new List<CheckListViewModel>());

            var controller = new CheckListOrderController(repositoryMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.NotNull(viewResult.Model);
            Assert.IsType<List<CheckListViewModel>>(viewResult.Model);
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
           var result = Assert.Throws<Exception>(() => controller.Index());
       
           // Assert
           Assert.Equal("Simulated error", result.Message);
       }

        [Fact]
        public void Index_ReturnsViewWithCorrectModelType()
        {
            // Arrange
            var repositoryMock = new Mock<ICheckListRepository>();
            repositoryMock.Setup(repo => repo.GetSomeOrderInfo())
                .Returns(new List<CheckListViewModel>());

            var controller = new CheckListOrderController(repositoryMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.IsType<List<CheckListViewModel>>(viewResult.Model);
        }
    }
}
