using Microsoft.AspNetCore.Identity;
using NoteListAPI.ServiceLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteListAPI.ServiceLayer.IServices
{
    public interface IRoleService
    {
        Task<IEnumerable<IdentityRole<int>>> GetAllRoles();
        Task<bool> CreateRoleAsync(CreateRole model);
        Task DeleteRoleAsync(int id);
    }
}
