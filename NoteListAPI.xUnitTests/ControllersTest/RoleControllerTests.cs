using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NoteListAPI.Controllers;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.xUnitTests.ControllersTest
{
    public class RoleControllerTests
    {
        #region Fields

        private readonly RoleController _controller;
        private readonly Mock<IRoleService> _mockRoleService;

        #endregion

        #region Constructor

        public RoleControllerTests()
        {
            _mockRoleService = new Mock<IRoleService>();
            _controller = new RoleController(_mockRoleService.Object);
        }

        #endregion

        #region GetAllRolesTests

        [Fact]
        public async Task GetAllRoles_ReturnsOkResult_WhenRolesExist()
        {
            // Arrange
            var roles = new List<IdentityRole<int>>
            {
            new IdentityRole<int> { Id = 1, Name = "Admin" },
            new IdentityRole<int> { Id = 2, Name = "User" }
        };
            _mockRoleService.Setup(service => service.GetAllRoles())
                .ReturnsAsync(roles);

            // Act
            var result = await _controller.GetAllRoles();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnRoles = Assert.IsType<List<IdentityRole<int>>>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(2, returnRoles.Count);
        }

        [Fact]
        public async Task GetAllRoles_ReturnsNotFound_WhenNoRolesExist()
        {
            // Arrange
            _mockRoleService.Setup(service => service.GetAllRoles())
                .ReturnsAsync((List<IdentityRole<int>>)null);

            // Act
            var result = await _controller.GetAllRoles();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("No role found", notFoundResult.Value);
        }

        #endregion

        #region CreateRoleTests

        [Fact]
        public async Task CreateRole_ReturnsOkResult_WhenRoleIsCreated()
        {
            // Arrange
            var model = new CreateRole { RoleName = "NewRole" };
            _mockRoleService.Setup(service => service.CreateRoleAsync(model))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.CreateRole(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("Role created successfully", okResult.Value);
        }

        [Fact]
        public async Task CreateRole_ReturnsBadRequest_WhenModelIsNull()
        {
            // Act
            var result = await _controller.CreateRole(null);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Failed to create role", badRequestResult.Value);
        }

        [Fact]
        public async Task CreateRole_ReturnsBadRequest_WhenRoleCreationFails()
        {
            // Arrange
            var model = new CreateRole { RoleName = "NewRole" };
            _mockRoleService.Setup(service => service.CreateRoleAsync(model))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.CreateRole(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Failed to create role", badRequestResult.Value);
        }

        #endregion

        #region DeleteRoleTests

        [Fact]
        public async Task DeleteRole_ReturnsOkResult_WhenRoleIsDeleted()
        {
            // Arrange
            var roleId = 1;
            _mockRoleService.Setup(service => service.DeleteRoleAsync(roleId))
                .ReturnsAsync(true);

            // Act
            var result = await _controller.DeleteRole(roleId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("Role deleted successfully", okResult.Value);
        }

        [Fact]
        public async Task DeleteRole_ReturnsBadRequest_WhenIdIsZero()
        {
            // Act
            var result = await _controller.DeleteRole(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Role does not exists", badRequestResult.Value);
        }

        [Fact]
        public async Task DeleteRole_ReturnsNotFound_WhenRoleDoesNotExist()
        {
            // Arrange
            var roleId = 1;
            _mockRoleService.Setup(service => service.DeleteRoleAsync(roleId))
                .ReturnsAsync(false);

            // Act
            var result = await _controller.DeleteRole(roleId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("Role does not exists", notFoundResult.Value);
        }

        #endregion
    }
}
