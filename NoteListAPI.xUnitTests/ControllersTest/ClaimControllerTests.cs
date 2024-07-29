using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NoteListAPI.Controllers;
using NoteListAPI.DomainLayer.Models;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.xUnitTests.ControllersTest
{
    public class ClaimControllerTests
    {
        #region Fields

        private readonly ClaimController _controller;
        private readonly Mock<IClaimService> _mockClaimService;

        #endregion

        #region Constructor

        public ClaimControllerTests()
        {
            _mockClaimService = new Mock<IClaimService>();
            _controller = new ClaimController(_mockClaimService.Object);
        }

        #endregion

        #region GetAllUserTests

        [Fact]
        public void GetAllUser_ReturnsOkResult_WhenUsersExist()
        {
            // Arrange
            var users = new List<UserViewModel>
        {
            new UserViewModel { Id = 1, UserName = "user1@gmail.com", Email = "user1@gmail.com" },
            new UserViewModel { Id = 2, UserName = "user2@gmail.com", Email = "user2@gmail.com" }
        };
            _mockClaimService.Setup(service => service.GetAllUser())
                .Returns(users);

            // Act
            var result = _controller.GetAllUser();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnUsers = Assert.IsType<List<UserViewModel>>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(2, returnUsers.Count);
        }

        [Fact]
        public void GetAllUser_ReturnsNotFound_WhenNoUsersExist()
        {
            // Arrange
            _mockClaimService.Setup(service => service.GetAllUser())
                .Returns((List<UserViewModel>)null);

            // Act
            var result = _controller.GetAllUser();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("No user found", notFoundResult.Value);
        }

        #endregion

        #region UserClaimsTests

        [Fact]
        public async Task UserClaims_ReturnsOkResult_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var user = new IdentityUser<int> { Id = userId, UserName = "user1@gmail.com", Email = "user1@gmail.com" };
            var claimsModel = new UserClaimViewModel { Id = userId, UserName = "user1@gmail.com", Email = "user1@gmail.com", Cliams = new List<UserClaim> { new UserClaim { ClaimType = "Role", IsSelected = true, isRoleClaimed = false } } };

            _mockClaimService.Setup(service => service.FindUserByIdAsync(userId))
                .ReturnsAsync(user);
            _mockClaimService.Setup(service => service.GetUserClaimsModel(user))
                .ReturnsAsync(claimsModel);

            // Act
            var result = await _controller.UserClaims(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnClaimsModel = Assert.IsType<UserClaimViewModel>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(userId, returnClaimsModel.Id);
        }

        [Fact]
        public async Task UserClaims_ReturnsBadRequest_WhenUserIdIsZero()
        {
            // Act
            var result = await _controller.UserClaims(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("User does not exist", badRequestResult.Value);
        }

        [Fact]
        public async Task UserClaims_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _mockClaimService.Setup(service => service.FindUserByIdAsync(userId))
                .ReturnsAsync((IdentityUser<int>)null);

            // Act
            var result = await _controller.UserClaims(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("User does not exist", notFoundResult.Value);
        }

        #endregion

        #region UpdateUserClaimsTests

        [Fact]
        public async Task UpdateUserClaims_ReturnsOkResult_WhenClaimsAreUpdatedSuccessfully()
        {
            // Arrange
            var userId = 1;
            var user = new IdentityUser<int> { Id = userId, UserName = "user1@gmail.com", Email = "user1@gmail.com" };
            var model = new UserClaimViewModel { Id = userId, UserName = "user1@gmail.com", Email = "user1@gmail.com", Cliams = new List<UserClaim> { new UserClaim { ClaimType = "Role", IsSelected = true, isRoleClaimed = false } } };

            _mockClaimService.Setup(service => service.FindUserByIdAsync(model.Id))
                .ReturnsAsync(user);
            _mockClaimService.Setup(service => service.UpdateUserClaimsAsync(model))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.UpdateUserClaims(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("User Claims update successfully", okResult.Value);
        }

        [Fact]
        public async Task UpdateUserClaims_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            _controller.ModelState.AddModelError("Error", "Invalid model");

            // Act
            var result = await _controller.UpdateUserClaims(new UserClaimViewModel());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Failed to update user claims", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateUserClaims_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var model = new UserClaimViewModel { Id = userId, UserName = "user1@gmail.com", Email = "user1@gmail.com", Cliams = new List<UserClaim> { new UserClaim { ClaimType = "Role", IsSelected = true, isRoleClaimed = false } } };

            _mockClaimService.Setup(service => service.FindUserByIdAsync(model.Id))
                .ReturnsAsync((IdentityUser<int>)null);

            // Act
            var result = await _controller.UpdateUserClaims(model);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("User does not exist", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateUserClaims_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var userId = 1;
            var user = new IdentityUser<int> { Id = userId, UserName = "user1@gmail.com", Email = "user1@gmail.com" };
            var model = new UserClaimViewModel { Id = userId, UserName = "user1@gmail.com", Email = "user1@gmail.com", Cliams = new List<UserClaim> { new UserClaim { ClaimType = "Role", IsSelected = true, isRoleClaimed = false } } };

            _mockClaimService.Setup(service => service.FindUserByIdAsync(model.Id))
                .ReturnsAsync(user);
            _mockClaimService.Setup(service => service.UpdateUserClaimsAsync(model))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.UpdateUserClaims(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Failed to update claims", badRequestResult.Value);
        }

        #endregion
    }
}
