using NoteListAPI.DomainLayer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteListAPI.RepositoryLayer.IRepositories
{
    public interface INoteRepository : IRepository<Note>
    {
    }
}
