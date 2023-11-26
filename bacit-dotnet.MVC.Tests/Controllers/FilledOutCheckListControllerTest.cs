using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Models.Composite;
using bacit_dotnet.MVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
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
                    ChecklistId = id,
                    ClutchCheck = "OK",
                    BrakeCheck = "OK",
                    DrumBearingCheck = "OK",
                    PTOCheck = "OK",
                    ChainTensionCheck = "OK",
                    WireCheck = "OK",
                    PinionBearingCheck = "OK",
                    ChainWheelKeyCheck = "OK",
                    HydraulicCylinderCheck = "OK",
                    HoseCheck = "OK",
                    HydraulicBlockTest = "OK",
                    TankOilChange = "OK",
                    GearboxOilChange = "OK",
                    RingCylinderSealsCheck = "OK",
                    BrakeCylinderSealsCheck = "OK",
                    WinchWiringCheck = "OK",
                    RadioCheck = "OK",
                    ButtonBoxCheck = "OK",
                    PressureSettings = "OK",
                    FunctionTest = "OK",
                    TractionForceKN = "OK",
                    BrakeForceKN = "OK",
                    Freeform = "Additional comments",
                    Sign = "John Doe",
                    CompletionDate = DateTime.Now, // Eksempel på en gyldig dato
                });

            var controller = new FilledOutCheckListController(mockRepository.Object);

            // Act
            var result = controller.Index(1);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<CheckListViewModel>(viewResult.Model);
            Assert.NotNull(model);
            Assert.Equal(1, model.ChecklistId);
            Assert.Equal("OK", model.ClutchCheck);
            Assert.Equal("OK", model.BrakeCheck);
            Assert.Equal("OK", model.DrumBearingCheck);
            // ... (legg til tilsvarende assert-statements for resten av sjekkpunktene)
            Assert.Equal("Additional comments", model.Freeform);
            Assert.Equal("John Doe", model.Sign);
            Assert.NotNull(model.CompletionDate);
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

        [Fact]
        public void Index_HandlesInvalidId_ReturnsNotFoundResult()
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

