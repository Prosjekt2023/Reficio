using Microsoft.AspNetCore.Mvc;
using Xunit;
using bacit_dotnet.MVC.Controllers;

namespace bacit_dotnet.MVC.Tests.Controllers
{
    public class SignOutControllerTests
    {
        [Fact]
        public void Index_GET_ReturnsViewResult()
        {
//Arrange
            var controller = new SignOutController();

//Act
            var result = controller.Index();
 
//Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            Assert.Null(viewResult.ViewName); //Optionally,checkfortheviewnameifit'sset
        }
    }
}