﻿using NoteListAPI.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteListAPI.ServiceLayer.Models
{
    public class UserRoleViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<UserRole> Roles { get; set; }
    }
}
