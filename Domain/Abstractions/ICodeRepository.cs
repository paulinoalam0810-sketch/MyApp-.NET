using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Abstractions
{
    public interface ICodeRepository<TEntity> where TEntity : class
    {
        Task<TEntity?> GetByCodeAsync(string code);
        Task<bool> ExistsWithCodeAsync(string code);
    }
}
