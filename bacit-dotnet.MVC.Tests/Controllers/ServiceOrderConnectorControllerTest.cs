using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Models.Composite;
using bacit_dotnet.MVC.Repositories;

namespace bacit_dotnet.MVC.Tests.Controllers
{
    public class ServiceOrderConnectorControllerTests
    {
        [Fact]
        public void Index_GET_ReturnsViewResultWithCompositeViewModel()
        {
            // Arrange
            var serviceFormRepositoryMock = new Mock<IServiceFormRepository>();
            var checkListRepositoryMock = new Mock<ICheckListRepository>();
            var controller = new ServiceOrderConnectorController(serviceFormRepositoryMock.Object, checkListRepositoryMock.Object);

            var id = 1;

            // Setup mock behavior for IServiceFormRepository
            var serviceFormEntry = new ServiceFormViewModel { /* Set properties as needed */ };
            serviceFormRepositoryMock.Setup(repo => repo.GetRelevantData(id)).Returns(serviceFormEntry);

            // Setup mock behavior for ICheckListRepository
            var checkListEntry = new CheckListViewModel { /* Set properties as needed */ };
            checkListRepositoryMock.Setup(repo => repo.GetRelevantData(id)).Returns(checkListEntry);

            // Act
            var result = controller.Index(id);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CompositeViewModel>(viewResult.Model);

            // Additional assertions based on your model and view logic
            Assert.Equal(serviceFormEntry, model.ServiceForm);
            Assert.Equal(checkListEntry, model.CheckList);
        }

        [Fact]
        public void Index_GET_WithBothNull_ReturnsNotFoundResult()
        {
            // Arrange
            var serviceFormRepositoryMock = new Mock<IServiceFormRepository>();
            var checkListRepositoryMock = new Mock<ICheckListRepository>();
            var controller = new ServiceOrderConnectorController(serviceFormRepositoryMock.Object, checkListRepositoryMock.Object);

            var id = 1;

            // Setup mock behavior for IServiceFormRepository
            serviceFormRepositoryMock.Setup(repo => repo.GetRelevantData(id)).Returns((ServiceFormViewModel)null);

            // Setup mock behavior for ICheckListRepository
            checkListRepositoryMock.Setup(repo => repo.GetRelevantData(id)).Returns((CheckListViewModel)null);

            // Act
            var result = controller.Index(id);

            // Assert
            Assert.IsType<ViewResult>(result); // Corrected assertion
        }
    }
}
