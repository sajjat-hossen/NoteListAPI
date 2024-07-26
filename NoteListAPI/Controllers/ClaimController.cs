using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "SuperAdmin,Admin")]

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

        #region GetAllUser

        [HttpGet("allUser")]
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

        #endregion

        #region UserClaims

        [HttpGet("userClaims/{id:int}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> UserClaims(int id)
        {
            if (id == 0)
            {
                return BadRequest("User does not exist");
            }

            var user = await _claimService.FindUserByIdAsync(id);

            if (user == null)
            {
                return NotFound("User does not exist");
            }

            var model = await _claimService.GetUserClaimsModel(user);

            return Ok(model);
        }

        #endregion

        #region UpdateUserClaim

        [HttpPost("UpdateUserClaims")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> UpdateUserClaims(UserClaimViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Failed to update user claims");
            }

            var user = await _claimService.FindUserByIdAsync(model.Id);

            if (user == null)
            {
                return NotFound("User does not exist");
            }

            var result = await _claimService.UpdateUserClaimsAsync(model);

            if (result == false)
            {
                return BadRequest("Failed to update claims");
            }

            return Ok("User Claims update successfully");
        }  

        #endregion
    }
}
