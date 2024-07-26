using Microsoft.AspNetCore.Identity;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.ServiceLayer.IServices
{
    public interface IRoleService
    {
        Task<IEnumerable<IdentityRole<int>>> GetAllRoles();
        Task<bool> CreateRoleAsync(CreateRole model);
        Task<bool> DeleteRoleAsync(int id);
    }
}
