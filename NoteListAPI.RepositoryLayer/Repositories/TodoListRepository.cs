using NoteListAPI.DomainLayer.Data;
using NoteListAPI.DomainLayer.Models;
using NoteListAPI.RepositoryLayer.IRepositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NoteListAPI.RepositoryLayer.Repositories
{
    public class TodoListRepository : Repository<TodoList>, ITodoListRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TodoListRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
