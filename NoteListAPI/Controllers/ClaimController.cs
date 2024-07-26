using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteListAPI.ServiceLayer.IServices;

namespace NoteListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimController : ControllerBase
    {
        #region Fields

        private readonly IClaimService _claimService;

        #endregion

        #region Constructor

        public ClaimController(IClaimService claimService)
        {
            _claimService = claimService;
        }

        #endregion

        [HttpGet("all")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]

        public IActionResult GetAllUser()
        {
            var users = _claimService.GetAllUser();

            if (users == null)
            {
                return NotFound("No user found");
            }

            return Ok(users);
        }
    }
}
