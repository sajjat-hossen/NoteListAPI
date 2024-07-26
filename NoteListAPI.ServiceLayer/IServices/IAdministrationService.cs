using Microsoft.AspNetCore.Identity;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.ServiceLayer.IServices
{
    public interface IAdministrationService
    {
        IEnumerable<UserViewModel> GetAllUser();
        Task<IdentityUser<int>> FindUserByIdAsync(int id);
        Task<IEnumerable<string>> GetUserRolesAsync(IdentityUser<int> user);
        Task<UserRoleViewModel> GetUserRolesModel(IdentityUser<int> user);
        Task<bool> UpdateUserRolesAsync(UserRoleViewModel model);
        Task<List<RoleClaimViewModel>> GetRoleClaimsAsync();
        Task<bool> UpdateRoleClaimsAsync(List<RoleClaimViewModel> models);
    }
}
