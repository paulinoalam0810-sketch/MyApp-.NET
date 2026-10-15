using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public interface IRepository<TEntity, TId> where TEntity : class  
    {
        Task<TEntity?> GetByIdAsync(TId id);
        Task<IEnumerable<TEntity>> GetAllAsync();
        Task AddAsync(TEntity entity);
        Task UpdateAsync(TEntity entity);
        Task DeleteAsync(TEntity entity);
        Task<int> SaveChangesAsync();
    }
}
