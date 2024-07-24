using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteListAPI.DomainLayer.Models;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;
using NoteListAPI.ServiceLayer.Services;

namespace NoteListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        #region Fields

        private readonly IRoleService _roleService;

        #endregion

        #region Constructor

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        #endregion

        #region GetAllRoles

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleService.GetAllRoles();

            if (roles != null)
            {
                return Ok(roles);
            }

            return NotFound("No role found");
        }

        #endregion

        #region CreateRole

        [HttpPost("add")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> CreateRole(CreateRole model)
        {
            if (model == null)
            {
                return BadRequest("Failed to create role");
            }

            var result = await _roleService.CreateRoleAsync(model);

            if (result == false)
            {
                return BadRequest("Failed to create role");

            }

            return Ok("Role created successfully");
        }

        #endregion
    }
}
