
namespace bacit_dotnet.MVC.Tests.Controllers;

public class ServiceOrderConnectorControllerTest
{
    
using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Models.Composite;
using bacit_dotnet.MVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class ServiceOrderConnectorControllerTests
{
    [Fact]
    public void Index_ReturnsViewWithModel_WhenValidId()
    {
        // Arrange
        var mockServiceFormRepository = new Mock<IServiceFormRepository>();
        var mockCheckListRepository = new Mock<ICheckListRepository>();
        var controller = new ServiceOrderConnectorController(mockServiceFormRepository.Object, mockCheckListRepository.Object);

        var serviceFormEntry = new ServiceFormEntry(); // Erstatt med reell klasse og data
        var checkListEntry = new CheckListEntry(); // Erstatt med reell klasse og data

        mockServiceFormRepository.Setup(repo => repo.GetRelevantData(It.IsAny<int>())).Returns(serviceFormEntry);
        mockCheckListRepository.Setup(repo => repo.GetRelevantData(It.IsAny<int>())).Returns(checkListEntry);

        // Act
        var result = controller.Index(1) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal(serviceFormEntry, (result.Model as CompositeViewModel)?.ServiceForm);
        Assert.Equal(checkListEntry, (result.Model as CompositeViewModel)?.CheckList);
    }

    [Fact]
    public void Index_ReturnsNotFound_WhenInvalidId()
    {
        // Arrange
        var mockServiceFormRepository = new Mock<IServiceFormRepository>();
        var mockCheckListRepository = new Mock<ICheckListRepository>();
        var controller = new ServiceOrderConnectorController(mockServiceFormRepository.Object, mockCheckListRepository.Object);

        mockServiceFormRepository.Setup(repo => repo.GetRelevantData(It.IsAny<int>())).Returns((ServiceFormEntry)null);

        // Act
        var result = controller.Index(2) as NotFoundResult;

        // Assert
        Assert.NotNull(result);
    }
}