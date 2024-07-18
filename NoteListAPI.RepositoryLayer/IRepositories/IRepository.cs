using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace NoteListAPI.RepositoryLayer.IRepositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<List<TEntity>> GetAllEntityFromDbAsync();
        Task<TEntity> GetEntityByIdAsync(Expression<Func<TEntity, bool>> filters);
        Task AddEntityAsync(TEntity entity);
        Task RemoveEntityAsync(TEntity entity);
        Task UpdateEntityAsync(TEntity entity);
    }
}