using EntityFrameworkCore.Extender.Abstract;

namespace EntityFrameworkCore.Extender;

public abstract class MyBaseEntity : IEntity, IEntityBase<Guid>,IEntityConcurrencyProps,IEntityDefaultProps
{
  public abstract Guid Id { get; set; }
  public abstract byte[] Version { get; set; }
  public abstract DateTime CreatedAt { get; set; }
  public abstract DateTime? UpdatedAt { get; set; }
}