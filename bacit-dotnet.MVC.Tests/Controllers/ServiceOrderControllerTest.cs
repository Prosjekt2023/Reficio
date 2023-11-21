using System.Collections.Generic;
using System.Linq;
using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Models.Composite;
using bacit_dotnet.MVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace bacit_dotnet.MVC.Tests.Controllers
{
    public class ServiceOrderControllerTests
    {
        [Fact]
        public void Index_GET_ReturnsViewResultWithServiceFormViewModelArray()
        {
            // Arrange
            var serviceFormRepositoryMock = new Mock<IServiceFormRepository>();
            var controller = new ServiceOrderController(serviceFormRepositoryMock.Object);

            var serviceFormEntries = new List<ServiceFormViewModel>
            {
                new ServiceFormViewModel { /* properties initialization */ },
                // Add more entries as needed
            };

            serviceFormRepositoryMock.Setup(repo => repo.GetSomeOrderInfo()).Returns(serviceFormEntries.ToArray());

            // Act
            var result = controller.Index();

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<ServiceFormViewModel[]>(viewResult.Model);

            Assert.Equal(serviceFormEntries.Count, model.Length);

            // You can add more assertions based on your actual model and view logic
        }
    }
}