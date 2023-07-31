using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Extender.Abstract;

public interface IRepository<TEntity> : IRepositoryAsync<TEntity>, IRepositorySync<TEntity>
   where TEntity : class, IEntity, new()
{
    public DbSet<TEntity> Table { get; }
}