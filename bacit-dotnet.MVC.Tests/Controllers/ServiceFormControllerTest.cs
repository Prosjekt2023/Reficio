using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Models.ServiceForm;
using bacit_dotnet.MVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;


    /*
     * NavnetPåConvention - KlasseNavnet_MetodeNavnet_ForventetResultat
     */
    public class ServiceFormControllerTests
    {
        [Fact]
        /*
         * Denne testen sjekker om Index-metoden, når den mottar en gyldig modell, omdirigerer til "Index"-handlingen i "ServiceOrder"-kontrolleren.
         */
        public void Index_WithValidModel_RedirectsToServiceOrderIndex()
        {
            // Arrange
            var repositoryMock = new Mock<ServiceFormRepository>();
            var controller = new ServiceFormController(repositoryMock.Object);
            var validModel = new ServiceFormViewModel { /* set valid properties */ };

            // Act
            var result = controller.Index(validModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);
            Assert.Equal("ServiceOrder", result.ControllerName);
        } 
        /*
         * Denne testen sjekker om Index-metoden, når den mottar en ugyldig modell, returnerer en visning med samme modell.
         */

        [Fact]
        public void Index_ModelStateNotValid_DoesNotCallRepositoryInsert_WithMock()
        {
            // Arrange
            var mockRepository = new Mock<ServiceFormRepository>();
            var controller = new ServiceFormController(mockRepository.Object);
            var invalidModel = new ServiceFormViewModel { /* set invalid properties */ };
            controller.ModelState.AddModelError("PropertyName", "Error Message");

            // Act
            var result = controller.Index(invalidModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(invalidModel, result.Model);

            // Verify
            mockRepository.Verify(repo => repo.Insert(It.IsAny<ServiceFormViewModel>()), Times.Never);
        }


        [Fact]
        /*
         * Denne testen sjekker om Index-metoden ikke kaller Insert-metoden på repository hvis modelltilstanden ikke er gyldig.
         */
        public void AnotherTest()
        {
            // Arrange
            var realRepository = new ServiceFormRepository(); // Bruk en ekte implementering
            var controller = new ServiceFormController(realRepository);
            var invalidModel = new ServiceFormViewModel { /* set invalid properties */ };
            controller.ModelState.AddModelError("PropertyName", "Error Message");

            // Act
            var result = controller.Index(invalidModel) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal(invalidModel, result.Model);

            // Verify
            // Du kan legge til en egendefinert metode i ServiceFormRepository for å telle antall ganger Insert blir kalt
            Assert.Equal(0, realRepository.NumberOfTimesInsertCalled); // Legg til dette som en egendefinert egenskap/metode på ServiceFormRepository
        }

    }
