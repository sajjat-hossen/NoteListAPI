using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NoteListAPI.Controllers;
using NoteListAPI.DomainLayer.Models;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteListAPI.xUnitTests.ControllersTest
{
    public class AdministrationControllerTests
    {
        #region Fields

        private readonly AdministrationController _administrationController;
        private readonly Mock<IAdministrationService> _mockAdministrationService;

        #endregion

        #region Constructor

        public AdministrationControllerTests()
        {
            _mockAdministrationService = new Mock<IAdministrationService>();
            _administrationController = new AdministrationController(_mockAdministrationService.Object);
        }

        #endregion

        #region GetAllUserTests

        [Fact]
        public void GetAllUser_ReturnsOkResult_WhenUsersExist()
        {
            // Arrange
            var users = new List<UserViewModel>
            {
                new UserViewModel{ Id = 1, UserName = "user1@gmail.com", Email = "user1@gmail.com" },
                new UserViewModel { Id = 2, UserName = "user2@gmail.com", Email = "user2@gmail.com" }
            };
            _mockAdministrationService.Setup(service => service.GetAllUser())
                .Returns(users);

            // Act
            var result = _administrationController.GetAllUser();

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
            _mockAdministrationService.Setup(service => service.GetAllUser())
                .Returns((List<UserViewModel>)null);

            // Act
            var result = _administrationController.GetAllUser();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("No user found", notFoundResult.Value);
        }

        #endregion

        #region UserRolesTests

        [Fact]
        public async Task UserRoles_ReturnsOkResult_WhenUserExists()
        {
            // Arrange
            var userId = 1;
            var user = new IdentityUser<int> 
            { 
                Id = userId, 
                UserName = "user1@gmail.com", 
                Email = "user1@gmail.com" 
            };
            var rolesModel = new UserRoleViewModel 
            { 
                Id = userId, 
                UserName = "user1@gmail.com", 
                Email = "user1@gmail.com", 
                Roles = new List<UserRole> 
                { 
                    new UserRole { RoleName = "Admin", IsSelected = true },
                    new UserRole { RoleName = "Sales", IsSelected = false }
                } 
            };

            _mockAdministrationService.Setup(service => service.FindUserByIdAsync(userId))
                .ReturnsAsync(user);
            _mockAdministrationService.Setup(service => service.GetUserRolesModel(user))
                .ReturnsAsync(rolesModel);

            // Act
            var result = await _administrationController.UserRoles(userId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnRolesModel = Assert.IsType<UserRoleViewModel>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(userId, returnRolesModel.Id);
        }

        [Fact]
        public async Task UserRoles_ReturnsBadRequest_WhenUserIdIsZero()
        {
            // Act
            var result = await _administrationController.UserRoles(0);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("User does not exist", badRequestResult.Value);
        }

        [Fact]
        public async Task UserRoles_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            _mockAdministrationService.Setup(service => service.FindUserByIdAsync(userId))
                .ReturnsAsync((IdentityUser<int>)null);

            // Act
            var result = await _administrationController.UserRoles(userId);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("User does not exist", notFoundResult.Value);
        }

        #endregion

        #region UpdateUserRolesTests

        [Fact]
        public async Task UpdateUserRoles_ReturnsOkResult_WhenRolesAreUpdatedSuccessfully()
        {
            // Arrange
            var userId = 1;
            var user = new IdentityUser<int>
            {
                Id = userId,
                UserName = "user1@gmail.com",
                Email = "user1@gmail.com"
            };
            var model = new UserRoleViewModel
            {
                Id = userId,
                UserName = "user1@gmail.com",
                Email = "user1@gmail.com",
                Roles = new List<UserRole>
                {
                    new UserRole { RoleName = "Admin", IsSelected = true },
                    new UserRole { RoleName = "Sales", IsSelected = false }
                }
            };

            _mockAdministrationService.Setup(service => service.FindUserByIdAsync(model.Id))
                .ReturnsAsync(user);
            _mockAdministrationService.Setup(service => service.UpdateUserRolesAsync(model))
                .ReturnsAsync(true);

            // Act
            var result = await _administrationController.UpdateUserRoles(model);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("User roles updated successfully", okResult.Value);
        }

        [Fact]
        public async Task UpdateUserRoles_ReturnsBadRequest_WhenModelIsInvalid()
        {
            // Arrange
            _administrationController.ModelState.AddModelError("Error", "Invalid model");

            // Act
            var result = await _administrationController.UpdateUserRoles(new UserRoleViewModel());

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Failed to update user roles", badRequestResult.Value);
        }

        [Fact]
        public async Task UpdateUserRoles_ReturnsNotFound_WhenUserDoesNotExist()
        {
            // Arrange
            var userId = 1;
            var model = new UserRoleViewModel
            {
                Id = userId,
                UserName = "user1@gmail.com",
                Email = "user1@gmail.com",
                Roles = new List<UserRole>
                {
                    new UserRole { RoleName = "Admin", IsSelected = true },
                    new UserRole { RoleName = "Sales", IsSelected = false }
                }
            };

            _mockAdministrationService.Setup(service => service.FindUserByIdAsync(model.Id))
                .ReturnsAsync((IdentityUser<int>)null);

            // Act
            var result = await _administrationController.UpdateUserRoles(model);

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("User does not exist", notFoundResult.Value);
        }

        [Fact]
        public async Task UpdateUserRoles_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var userId = 1;
            var user = new IdentityUser<int>
            {
                Id = userId,
                UserName = "user1@gmail.com",
                Email = "user1@gmail.com"
            };
            var model = new UserRoleViewModel
            {
                Id = userId,
                UserName = "user1@gmail.com",
                Email = "user1@gmail.com",
                Roles = new List<UserRole>
                {
                    new UserRole { RoleName = "Admin", IsSelected = true },
                    new UserRole { RoleName = "Sales", IsSelected = false }
                }
            };

            _mockAdministrationService.Setup(service => service.FindUserByIdAsync(model.Id))
                .ReturnsAsync(user);
            _mockAdministrationService.Setup(service => service.UpdateUserRolesAsync(model))
                .ReturnsAsync(false);

            // Act
            var result = await _administrationController.UpdateUserRoles(model);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Failed to update user roles", badRequestResult.Value);
        }

        #endregion

        #region GetRoleClaimsTests

        [Fact]
        public async Task GetRoleClaims_ReturnsOkResult_WhenRoleClaimsExist()
        {
            // Arrange
            var roleClaims = new List<RoleClaimViewModel>
            {
                new RoleClaimViewModel 
                { 
                    RoleName = "Admin",
                    RoleClaims = new List<RoleClaim>
                    {
                        new RoleClaim { ClaimType = "View", IsSelected = true},
                        new RoleClaim { ClaimType = "Edit", IsSelected = true},
                        new RoleClaim { ClaimType = "Delete", IsSelected = false}
                    }
                },
                new RoleClaimViewModel
                {
                    RoleName = "Sales",
                    RoleClaims = new List<RoleClaim>
                    {
                        new RoleClaim { ClaimType = "View", IsSelected = true},
                        new RoleClaim { ClaimType = "Delete", IsSelected = false}
                    }
                }
            };
            _mockAdministrationService.Setup(service => service.GetRoleClaimsAsync())
                .ReturnsAsync(roleClaims);

            // Act
            var result = await _administrationController.GetRoleClaims();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnRoleClaims = Assert.IsType<List<RoleClaimViewModel>>(okResult.Value);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal(2, returnRoleClaims.Count);
        }

        [Fact]
        public async Task GetRoleClaims_ReturnsNotFound_WhenNoRoleClaimsExist()
        {
            // Arrange
            _mockAdministrationService.Setup(service => service.GetRoleClaimsAsync())
                .ReturnsAsync((List<RoleClaimViewModel>)null);

            // Act
            var result = await _administrationController.GetRoleClaims();

            // Assert
            var notFoundResult = Assert.IsType<NotFoundObjectResult>(result);

            Assert.Equal(StatusCodes.Status404NotFound, notFoundResult.StatusCode);
            Assert.Equal("Role Claims does not exist", notFoundResult.Value);
        }

        #endregion

        #region UpdateRoleClaimsTests

        [Fact]
        public async Task UpdateRoleClaims_ReturnsOkResult_WhenRoleClaimsAreUpdatedSuccessfully()
        {
            // Arrange
            var roleClaims = new List<RoleClaimViewModel>
            {
                new RoleClaimViewModel 
                {
                    RoleName = "Admin",
                    RoleClaims = new List<RoleClaim>
                    {
                        new RoleClaim { ClaimType = "View", IsSelected = true},
                        new RoleClaim { ClaimType = "Edit", IsSelected = true}
                    }
                },
                new RoleClaimViewModel 
                {
                    RoleName = "Sales",
                    RoleClaims = new List<RoleClaim>
                    {
                        new RoleClaim { ClaimType = "View", IsSelected = true},
                        new RoleClaim { ClaimType = "Edit", IsSelected = true},
                        new RoleClaim { ClaimType = "Delete", IsSelected = false}
                    }
                }
            };

            _mockAdministrationService.Setup(service => service.UpdateRoleClaimsAsync(roleClaims))
                .ReturnsAsync(true);

            // Act
            var result = await _administrationController.UpdateRoleClaims(roleClaims);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);

            Assert.Equal(StatusCodes.Status200OK, okResult.StatusCode);
            Assert.Equal("Role claims updated successfully", okResult.Value);
        }

        [Fact]
        public async Task UpdateRoleClaims_ReturnsBadRequest_WhenUpdateFails()
        {
            // Arrange
            var roleClaims = new List<RoleClaimViewModel>
            {
                new RoleClaimViewModel
                {
                    RoleName = "Admin",
                    RoleClaims = new List<RoleClaim>
                    {
                        new RoleClaim { ClaimType = "View", IsSelected = true},
                        new RoleClaim { ClaimType = "Edit", IsSelected = true}
                    }
                },
                new RoleClaimViewModel
                {
                    RoleName = "Sales",
                    RoleClaims = new List<RoleClaim>
                    {
                        new RoleClaim { ClaimType = "View", IsSelected = true},
                        new RoleClaim { ClaimType = "Edit", IsSelected = true},
                        new RoleClaim { ClaimType = "Delete", IsSelected = false}
                    }
                }
            };

            _mockAdministrationService.Setup(service => service.UpdateRoleClaimsAsync(roleClaims))
                .ReturnsAsync(false);

            // Act
            var result = await _administrationController.UpdateRoleClaims(roleClaims);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);

            Assert.Equal(StatusCodes.Status400BadRequest, badRequestResult.StatusCode);
            Assert.Equal("Failed to update role claims", badRequestResult.Value);
        }

        #endregion
    }
}
