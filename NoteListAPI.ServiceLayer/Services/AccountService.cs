using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;
using System.Security.Claims;

namespace NoteListAPI.ServiceLayer.Services
{
    public class AccountService : IAccountService
    {
        #region Fields

        private readonly UserManager<IdentityUser<int>> _userManager;
        private readonly SignInManager<IdentityUser<int>> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Constructor

        public AccountService(UserManager<IdentityUser<int>> userManager, SignInManager<IdentityUser<int>> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region CreateAccountAsync

        public async Task<IdentityResult> CreateAccountAsync(RegisterModel model)
        {
            var user = new IdentityUser<int>
            {
                UserName = model.Email,
                Email = model.Email
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            return result;

        }

        #endregion

        #region SignInAccountAsync

        public async Task SignInAccountAsync(RegisterModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            await _signInManager.SignInAsync(user, isPersistent: false);
        }

        #endregion

        #region PasswordSignInAccountAsync

        public async Task<SignInResult> PasswordSignInAccountAsync(LoginModel model)
        {
            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model
                .RememberMe, lockoutOnFailure: false);

            return result;
        }

        #endregion

        #region SignOutAccountAsync

        public async Task SignOutAccountAsync()
        {
            await _signInManager.SignOutAsync();
        }

        #endregion

        #region ChangePassword

        public async Task<IdentityResult> ChangePassword(ChangePasswordModel model)
        {
            var logedUserId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var logedUser = await _userManager.FindByIdAsync(logedUserId);

            var restul = await _userManager.ChangePasswordAsync(logedUser, model.OldPassword, model.NewPassword);

            if (!restul.Succeeded)
            {
                return restul;
            }

            await _signInManager.RefreshSignInAsync(logedUser);

            return restul;

        }

        #endregion
    }
}
