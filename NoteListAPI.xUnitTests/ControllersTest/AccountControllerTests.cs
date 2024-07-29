using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NoteListAPI.Controllers;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.xUnitTests.ControllersTest
{
    public class AccountControllerTests
    {
        #region Fields

        private readonly AccountController _controller;
        private readonly Mock<IAccountService> _mockAccountService;

        #endregion

        #region Constructor

        public AccountControllerTests()
        {
            _mockAccountService = new Mock<IAccountService>();
            _controller = new AccountController(_mockAccountService.Object);
        }

        #endregion

        #region RegisterTests

        [Fact]
        public async Task Register_ReturnsOkResult_WhenAccountIsCreated()
        {
            // Arrange
            var model = new RegisterModel { Email = "user1@domain.com", Password = "password1" };
            _mockAccountService.Setup(service => service.CreateAccountAsync(model))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.Register(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("Account created successfully", okResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var model = new RegisterModel { Email = "user1@gmail.com", Password = "password1" };
            _controller.ModelState.AddModelError("Error", "Invalid model");

            // Act
            var result = await _controller.Register(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Failed to create account", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenModelIsNull()
        {
            // Act
            var result = await _controller.Register(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Failed to create account", badRequestResult.Value);
        }

        [Fact]
        public async Task Register_ReturnsBadRequest_WhenAccountCreationFails()
        {
            // Arrange
            var model = new RegisterModel { Email = "user1@gmail.com", Password = "password1" };
            _mockAccountService.Setup(service => service.CreateAccountAsync(model))
                .ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _controller.Register(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Failed to create account", badRequestResult.Value);
        }

        #endregion

        #region LoginTests

        [Fact]
        public async Task Login_ReturnsOkResult_WhenLoginIsSuccessful()
        {
            // Arrange
            var model = new LoginModel { Email = "user1@gmail.com", Password = "password1" };
            _mockAccountService.Setup(service => service.PasswordSignInAccountAsync(model))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Success);

            // Act
            var result = await _controller.Login(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("User logged in successfully", okResult.Value);
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var model = new LoginModel { Email = "user1@gmail.com", Password = "password1" };
            _controller.ModelState.AddModelError("Error", "Invalid model");

            // Act
            var result = await _controller.Login(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Failed to login", badRequestResult.Value);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenLoginFails()
        {
            // Arrange
            var model = new LoginModel { Email = "user1@gmail.com", Password = "password1" };
            _mockAccountService.Setup(service => service.PasswordSignInAccountAsync(model))
                .ReturnsAsync(Microsoft.AspNetCore.Identity.SignInResult.Failed);

            // Act
            var result = await _controller.Login(model);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);

            Assert.Equal(StatusCodes.Status401Unauthorized, unauthorizedResult.StatusCode);
            Assert.Equal("Invalid login attempt", unauthorizedResult.Value);
        }

        #endregion

        #region LogoutTests

        [Fact]
        public async Task Logout_ReturnsOkResult()
        {
            // Act
            var result = await _controller.Logout();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("User logout successfully", okResult.Value);
        }

        #endregion

        #region ChangePasswordTests

        [Fact]
        public async Task ChangePassword_ReturnsOkResult_WhenPasswordIsChanged()
        {
            // Arrange
            var model = new ChangePasswordModel { OldPassword = "old", NewPassword = "new" };
            _mockAccountService.Setup(service => service.ChangePasswordAsync(model))
                .ReturnsAsync(IdentityResult.Success);

            // Act
            var result = await _controller.ChangePassword(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("Password changed successfully", okResult.Value);
        }

        [Fact]
        public async Task ChangePassword_ReturnsBadRequest_WhenModelStateIsInvalid()
        {
            // Arrange
            var model = new ChangePasswordModel { OldPassword = "old", NewPassword = "new" };
            _controller.ModelState.AddModelError("Error", "Invalid model");

            // Act
            var result = await _controller.ChangePassword(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Failed to change password", badRequestResult.Value);
        }

        [Fact]
        public async Task ChangePassword_ReturnsUnauthorized_WhenPasswordChangeFails()
        {
            // Arrange
            var model = new ChangePasswordModel { OldPassword = "old", NewPassword = "new" };
            _mockAccountService.Setup(service => service.ChangePasswordAsync(model)).ReturnsAsync(IdentityResult.Failed());

            // Act
            var result = await _controller.ChangePassword(model);

            // Assert
            var unauthorizedResult = Assert.IsType<UnauthorizedObjectResult>(result);

            Assert.Equal(StatusCodes.Status401Unauthorized, unauthorizedResult.StatusCode);
            Assert.Equal("Invalid change password attempt", unauthorizedResult.Value);
        }

        #endregion
    }
}
