using System.Collections.Generic;
using bacit_dotnet.MVC.Controllers;
using bacit_dotnet.MVC.Entities;
using bacit_dotnet.MVC.Models.Users;
using bacit_dotnet.MVC.Repositories;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace bacit_dotnet.MVC.Tests.Controllers
{
    public class UsersControllerTests
    {
        // Test to check if the Index method returns a view with the correct model.
        [Fact]
        public void Index_ReturnsViewWithModel()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUsers()).Returns(new List<UserEntity>());
            var controller = new UsersController(userRepositoryMock.Object);

            // Act
            var result = controller.Index(null) as ViewResult;

            // Assert
            Assert.NotNull(result);
            Assert.IsType<UserViewModel>(result.Model);
        }

        // Test to check if the Save method adds a new user when the user does not exist.
        [Fact]
        public void Save_AddsNewUser_WhenUserDoesNotExist()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUsers()).Returns(new List<UserEntity>());
            var controller = new UsersController(userRepositoryMock.Object);

            var newUserViewModel = new UserViewModel
            {
                Name = "New User",
                Email = "newuser@example.com",
                IsAdmin = true
            };

            // Act
            var result = controller.Save(newUserViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            userRepositoryMock.Verify(repo => repo.Add(It.IsAny<UserEntity>()), Times.Once);
            userRepositoryMock.Verify(repo => repo.Update(It.IsAny<UserEntity>(), It.IsAny<List<string>>()), Times.Never);
        }

        // Test to check if the Save method updates an existing user when the user already exists.
        [Fact]
        public void Save_UpdatesUser_WhenUserExists()
        {
            // Arrange
            var existingUser = new UserEntity { Name = "Existing User", Email = "existinguser@example.com" };

            var userRepositoryMock = new Mock<IUserRepository>();
            userRepositoryMock.Setup(repo => repo.GetUsers()).Returns(new List<UserEntity> { existingUser });
            var controller = new UsersController(userRepositoryMock.Object);

            var updatedUserViewModel = new UserViewModel
            {
                Name = "Updated User",
                Email = "existinguser@example.com",
                IsAdmin = false
            };

            // Act
            var result = controller.Save(updatedUserViewModel) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            userRepositoryMock.Verify(repo => repo.Add(It.IsAny<UserEntity>()), Times.Never);
            userRepositoryMock.Verify(repo => repo.Update(It.IsAny<UserEntity>(), It.IsAny<List<string>>()), Times.Once);
        }

        // Test to check if the Delete method removes a user.
        [Fact]
        public void Delete_RemovesUser()
        {
            // Arrange
            var userRepositoryMock = new Mock<IUserRepository>();
            var controller = new UsersController(userRepositoryMock.Object);

            var userEmailToDelete = "user@example.com";

            // Act
            var result = controller.Delete(userEmailToDelete) as RedirectToActionResult;

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Index", result.ActionName);

            userRepositoryMock.Verify(repo => repo.Delete(userEmailToDelete), Times.Once);
        }
    }
}
