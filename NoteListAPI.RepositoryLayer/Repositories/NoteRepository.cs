using NoteListAPI.DomainLayer.Data;
using NoteListAPI.DomainLayer.Models;
using NoteListAPI.RepositoryLayer.IRepositories;

namespace NoteListAPI.RepositoryLayer.Repositories
{
    public class NoteRepository : Repository<Note>, INoteRepository
    {
        #region Fields

        private readonly ApplicationDbContext _dbContext;

        #endregion

        #region Constructor

        public NoteRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }

        #endregion

    }
}
