using System.Collections.Generic;
using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Models.Composite;
using bacit_dotnet.MVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

public class CheckListControllerTest
{
    [Fact]
    public void Index_Get_ReturnsView()
    {
        // Arrange
        var repositoryMock = new Mock<ICheckListRepository>();
        var controller = new CheckListController(repositoryMock.Object);

        // Act
        var result = controller.Index();

        // Assert
        Assert.IsType<ViewResult>(result);
    }

    [Fact]
    public void Index_Post_ValidModelState_RedirectsToFilledOutCheckList()
    {
        // Arrange
        var repositoryMock = new Mock<ICheckListRepository>();
        repositoryMock.Setup(repo => repo.Insert(It.IsAny<CheckListViewModel>()))
            .Returns(1);

        var controller = new CheckListController(repositoryMock.Object);
        var validModel = new CheckListViewModel();

        // Act
        var result = controller.Index(validModel) as RedirectToActionResult;

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Index", result.ActionName);
        Assert.Equal("FilledOutCheckList", result.ControllerName);
        Assert.Equal(1, result.RouteValues["id"]);
    }

    [Fact]
    public void Index_Post_InvalidModelState_ReturnsViewWithModel()
    {
        // Arrange
        var repositoryMock = new Mock<ICheckListRepository>();
        var controller = new CheckListController(repositoryMock.Object);
        controller.ModelState.AddModelError("PropertyName", "Error Message");
        var invalidModel = new CheckListViewModel();

        // Act
        var result = controller.Index(invalidModel) as ViewResult;

        // Assert
        Assert.NotNull(result);
        Assert.IsType<CheckListViewModel>(result.Model);
    }
}