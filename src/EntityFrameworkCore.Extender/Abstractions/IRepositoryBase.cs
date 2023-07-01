using Microsoft.EntityFrameworkCore;

namespace EntityFrameworkCore.Extender.Abstractions;

public interface IRepository<TContext, TEntity> where TContext : DbContext
   where TEntity : IEntityBase
{
   
}