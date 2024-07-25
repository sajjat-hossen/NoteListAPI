using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        #region Fields

        private readonly IAccountService _accountService;

        #endregion

        #region Constructor

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #endregion

        #region Register

        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Register(RegisterModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Failed to create account");
            }

            if (model == null)
            {
                return BadRequest("Failed to create account");
            }

            var result = await _accountService.CreateAccountAsync(model);

            if (result.Succeeded == false)
            {
                return BadRequest("Failed to create account");
            }

            await _accountService.SignInAccountAsync(model);

            return Ok("Account created successfully");
        }

        #endregion

        #region Login

        [HttpPost("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> Login(LoginModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Failed to login");
            }

            var result = await _accountService.PasswordSignInAccountAsync(model);

            if (!result.Succeeded)
            {
                return Unauthorized("Invalid login attempt");
            }

            return Ok("User logged in successfully");
        }

        #endregion

        #region Logout

        [HttpPost("logout")]
        [ProducesResponseType(StatusCodes.Status200OK)]

        public async Task<IActionResult> Logout()
        {
            await _accountService.SignOutAccountAsync();

            return Ok("User logged out successfully");
        }

        #endregion

        #region ChangePassword

        [HttpPost("changePassword")]

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]

        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Failed to change password");
            }

            var result = await _accountService.ChangePassword(model);

            if (!result.Succeeded)
            {
                return Unauthorized("Invalid change password attempt");
            }

            return Ok("Password changed successfully");
        }

        #endregion
    }
}
