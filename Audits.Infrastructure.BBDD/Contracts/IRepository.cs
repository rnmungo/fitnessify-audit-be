using System.Linq.Expressions;
using Audits.Domain.Contracts;

namespace Audits.Infrastructure.BBDD.Contracts
{
    public interface IRepository<TEntity, TKey>
        where TEntity : IBaseEntity<TKey>, ISoftDeleteEntity
        where TKey : struct
    {
        Task CreateAsync(TEntity entity);
        Task DeleteAsync(TKey id);
        IQueryable<TEntity> GetAll();
        IQueryable<TEntity> GetByCondition(Expression<Func<TEntity, bool>> expression);
        Task<TEntity> GetByIdAsync(TKey id);
        Task UpdateAsync(TEntity entity);
    }
}
