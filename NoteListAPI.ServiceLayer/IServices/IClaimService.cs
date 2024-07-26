using Microsoft.AspNetCore.Identity;
using NoteListAPI.ServiceLayer.Models;
using System.Security.Claims;

namespace NoteListAPI.ServiceLayer.IServices
{
    public interface IClaimService
    {
        IEnumerable<UserViewModel> GetAllUser();
        Task<IdentityUser<int>> FindUserByIdAsync(int id);
        Task<IEnumerable<Claim>> GetUserClaimsAsync(IdentityUser<int> user);
        Task<UserClaimViewModel> GetUserClaimsModel(IdentityUser<int> user);
        Task<bool> UpdateUserClaimsAsync(UserClaimViewModel model);
    }
}
