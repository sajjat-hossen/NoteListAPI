﻿using NoteListAPI.DomainLayer.Models;

namespace NoteListAPI.ServiceLayer.Models
{
    public class UserClaimViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public List<UserClaim> Cliams { get; set; }
    }
}
