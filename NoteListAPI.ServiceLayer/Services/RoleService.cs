using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NoteListAPI.ServiceLayer.IServices;
using NoteListAPI.ServiceLayer.Models;

namespace NoteListAPI.ServiceLayer.Services
{
    public class RoleService : IRoleService
    {
        #region Fields

        private readonly RoleManager<IdentityRole<int>> _roleManager;

        #endregion

        #region Constructor

        public RoleService(RoleManager<IdentityRole<int>> roleManager)
        {
            _roleManager = roleManager;
        }

        #endregion

        #region GetAllRoles

        public async Task<IEnumerable<IdentityRole<int>>> GetAllRoles()
        {
            return await _roleManager.Roles.ToListAsync();
        }

        #endregion

        #region CreateRoleAsync

        public async Task<bool> CreateRoleAsync(CreateRole model)
        {
            if (string.IsNullOrEmpty(model.RoleName))
            {
                return false;
            }

            var roleExist = await _roleManager.RoleExistsAsync(model.RoleName);

            if (roleExist)
            {
                return false;
            }

            var result = await _roleManager.CreateAsync(new IdentityRole<int>(model.RoleName));

            if (!result.Succeeded)
            {
                return false;
            }

            return true;

        }

        #endregion

        #region DeleteRoleAsync

        public async Task<bool> DeleteRoleAsync(int id)
        {
            var role = await _roleManager.FindByIdAsync(id.ToString());

            if (role == null)
            {
                return false;
            }
            
            await _roleManager.DeleteAsync(role);

            return true;
        }

        #endregion
    }
}
