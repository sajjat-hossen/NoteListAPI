using Microsoft.AspNetCore.Identity;
using NoteListAPI.ServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteListAPI.ServiceLayer.IServices
{
    public interface IAccountService
    {
        Task<IdentityResult> CreateAccountAsync(RegisterModel model);
        //Task SignInAccountAsync(RegisterViewModel model);
        //Task<SignInResult> PasswordSignInAccountAsync(LoginViewModel model);
        //Task SignOutAccountAsync();
        //Task<IdentityResult> ChangePassword(ChangePasswordViewModel model);
    }
}
