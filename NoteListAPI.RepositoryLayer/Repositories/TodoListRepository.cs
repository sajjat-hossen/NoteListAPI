using NoteListAPI.DomainLayer.Data;
using NoteListAPI.DomainLayer.Models;
using NoteListAPI.RepositoryLayer.IRepositories;

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
