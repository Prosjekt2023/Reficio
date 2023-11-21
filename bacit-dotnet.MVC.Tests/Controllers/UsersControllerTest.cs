using System.Collections.Generic;
using System.Linq;
using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Entities;
using bacit_dotnet.MVC.Models.Users;
using bacit_dotnet.MVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace bacit_dotnet.MVC.Tests.Controllers
{
    public class UsersControllerTests
    {
        [Fact]
        public void Index_GET_ReturnsViewResultWithUserViewModel()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var controller = new UsersController(userRepositoryMock.Object);

            var users = new List<UserEntity>
            {
                new UserEntity { Name = "John", Email = "john@example.com", IsAdmin = false },
                new UserEntity { Name = "Admin", Email = "admin@example.com", IsAdmin = true }
            };

            userRepositoryMock.Setup(repo => repo.GetUsers()).Returns(users);

            // Act
            var result = controller.Index(null);

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsType<UserViewModel>(viewResult.Model);

            Assert.Equal(users.Count, model.Users.Count);

            // You can add more assertions based on your actual model and view logic
        }
    }
}