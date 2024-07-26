using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Services;

namespace NoteListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministrationController : ControllerBase
    {
        #region Fields

        private readonly IAdministrationService _administrationService;

        #endregion

        #region Constructor

        public AdministrationController(IAdministrationService administrationService)
        {
            _administrationService = administrationService;
        }

        #endregion

        #region GetAllUser

        [HttpGet("allUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetAllUser()
        {
            var users = _administrationService.GetAllUser();

            if (users == null)
            {
                return NotFound("No user found");
            }

            return Ok(users);
        }

        #endregion

        #region UserRoles

        [HttpGet("userRoles/{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> UserRoles(int id)
        {
            if (id == 0)
            {
                return BadRequest("User does not exist");
            }

            var user = await _administrationService.FindUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User does not exist");
            }

            var model = await _administrationService.GetUserRolesModel(user);

            return Ok(model);
        }

        #endregion
    }
}
