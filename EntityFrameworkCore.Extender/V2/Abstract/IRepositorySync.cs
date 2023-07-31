using System.Linq.Expressions;

namespace EntityFrameworkCore.Extender.Abstract;

public interface IRepositorySync<TEntity>
  where TEntity : class, IEntity, new()
{
  IQueryable<TEntity> GetAll();
  IQueryable<TEntity> Get(Expression<Func<TEntity, bool>> predicate);
  IQueryable<TEntity> Get<TKey>(
    Expression<Func<TEntity, bool>>? predicateExpression = null,
    Expression<Func<TEntity, TKey>>? orderByExpression = null,
    bool isDescending = false,
    int? skip = null,
    int? take = null,
    params Expression<Func<TEntity, object>>[] includeProperties);
  TEntity? Find(params object[] keys);
  TEntity? SingleOrDefault();
  TEntity? SingleOrDefault(Expression<Func<TEntity, bool>> predicate);
  TEntity? SingleOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
  TEntity? FirstOrDefault();
  TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate);
  TEntity? FirstOrDefault(Expression<Func<TEntity, bool>> predicate, params Expression<Func<TEntity, object>>[] includeProperties);
  bool Any();
  bool Any(Expression<Func<TEntity, bool>> predicate);
  int Count();
  int Count(Expression<Func<TEntity, bool>> predicate);
  void Add(TEntity entity);
  void AddRange(IEnumerable<TEntity> entities);
  void Update(TEntity entity);
  void UpdateRange(IEnumerable<TEntity> entities);
  void Delete(TEntity entity);
  void RemoveRange(IEnumerable<TEntity> entities);
  bool HasChanges();


}