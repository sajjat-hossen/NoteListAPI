using NoteListAPI.DomainLayer.Models;

namespace NoteListAPI.ServiceLayer.Models
{
    public class RoleClaimViewModel
    {
        public string RoleName { get; set; }
        public List<RoleClaim> RoleClaims { get; set; }
    }
}
