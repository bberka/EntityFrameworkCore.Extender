namespace EntityFrameworkCore.Extender.Abstract;

public interface IEntitySoftDeleteProps 
{
  public DateTime? DeletedAt { get; set; }

}