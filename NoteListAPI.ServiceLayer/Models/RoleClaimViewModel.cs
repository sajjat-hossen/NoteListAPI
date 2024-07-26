using NoteListAPI.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteListAPI.ServiceLayer.Models
{
    public class RoleClaimViewModel
    {
        public string RoleName { get; set; }
        public List<RoleClaim> RoleClaims { get; set; }
    }
}
