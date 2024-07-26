using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin")]

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

        #region DeleteRole

        [HttpDelete("delete/{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> DeleteRole(int id)
        {
            if (id == 0)
            {
                return BadRequest("Role does not exists");
            }

            var result = await _roleService.DeleteRoleAsync(id);

            if (result == false)
            {
                return NotFound("Role does not exists");
            }

            return Ok("Role deleted successfully");
        }

        #endregion
    }
}
