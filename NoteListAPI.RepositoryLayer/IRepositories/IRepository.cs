﻿using System.Linq.Expressions;

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