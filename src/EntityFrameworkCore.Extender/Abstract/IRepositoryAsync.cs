using System.Linq.Expressions;

namespace EntityFrameworkCore.Extender.Abstract;

public interface IRepositoryAsync<TEntity>
  where TEntity : class, IEntity, new()
{
  Task<IQueryable<TEntity>> GetAllAsync();
  Task<IQueryable<TEntity>> GetAsync(Expression<Func<TEntity, bool>> predicate);
  Task<IQueryable<TEntity>> GetAsync<TKey>(
    Expression<Func<TEntity, bool>>? predicateExpression = null,
    Expression<Func<TEntity, TKey>>? orderByExpression = null,
    bool isDescending = false,
    int? skip = null,
    int? take = null,
    params Expression<Func<TEntity, object>>[] includeProperties);
  Task<TEntity?> FindAsync(params object[] keys);

  Task<TEntity?> SingleOrDefaultAsync();
  Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
  Task<TEntity?> SingleOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
  Task<TEntity?> FirstOrDefaultAsync();
  Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate);
  Task<TEntity?> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
  Task<bool> AnyAsync();
  Task<bool> AnyAsync(Expression<Func<TEntity, bool>> predicate);
  Task<int> CountAsync();
  Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate);
  Task AddAsync(TEntity entity);
  Task AddRangeAsync(IEnumerable<TEntity> entities);
  Task<bool> HasChangesAsync();


}