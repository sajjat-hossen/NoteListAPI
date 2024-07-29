using Microsoft.AspNetCore.Identity;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.ServiceLayer.IServices
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateAccountAsync(RegisterModel model);
        Task SignInAccountAsync(RegisterModel model);
        Task<SignInResult> PasswordSignInAccountAsync(LoginModel model);
        Task SignOutAccountAsync();
        Task<IdentityResult> ChangePasswordAsync(ChangePasswordModel model);
    }
}
