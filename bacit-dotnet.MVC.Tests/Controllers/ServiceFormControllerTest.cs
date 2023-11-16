using System;
using System.Collections.Generic;
using System.Linq;
using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Models.ServiceForm;
using bacit_dotnet.MVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace YourNamespace.Tests
{
    public class ServiceFormControllerTests
    {
        [Fact]
        public void Index_ReturnsViewResult()
        {
            // Arrange
            var repositoryMock = new Mock<ServiceFormRepository>();
            var controller = new ServiceFormController(repositoryMock.Object);

            // Act
            var result = controller.Index();

            // Assert
            Assert.IsType<ViewResult>(result);
        }

        [Fact]
        public void IndexPost_ValidModel_RedirectsToServiceOrderIndex()
        {
            // Arrange
            var repositoryMock = new Mock<ServiceFormRepository>();
            var controller = new ServiceFormController(repositoryMock.Object);
            var validModel = new ServiceFormViewModel
            {
                // Sett gyldige egenskaper for modellen her
            };

            // Act
            var result = controller.Index(validModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("ServiceOrder", result.ControllerName);
        }

        [Fact]
        public void IndexPost_InvalidModel_ReturnsViewResult()
        {
            // Arrange
            var repositoryMock = new Mock<ServiceFormRepository>();
            var controller = new ServiceFormController(repositoryMock.Object);
            var invalidModel = new ServiceFormViewModel
            {
                // Sett ugyldige egenskaper for modellen her
            };
            controller.ModelState.AddModelError("Egenskapsnavn", "Feilmelding");

            // Act
            var result = controller.Index(invalidModel);

            // Assert
            Assert.IsType<ViewResult>(result);
            Assert.Equal(invalidModel, ((ViewResult)result).Model);
        }
    }
}
