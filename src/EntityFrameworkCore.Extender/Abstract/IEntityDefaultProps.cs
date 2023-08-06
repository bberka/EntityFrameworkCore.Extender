namespace EntityFrameworkCore.Extender.Abstract;

public interface IEntityDefaultProps 
{
  public DateTime CreatedAt { get; set; }
  public DateTime? UpdatedAt { get; set; }
  
}