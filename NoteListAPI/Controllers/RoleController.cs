using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteListAPI.ServiceLayer.IServices;
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
    }
}
