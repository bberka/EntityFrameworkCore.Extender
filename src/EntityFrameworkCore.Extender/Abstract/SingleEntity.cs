using System.ComponentModel.DataAnnotations;

namespace EntityFrameworkCore.Extender.Abstract;

public abstract class SingleEntity
{
  private bool _key = true;
  /// <summary>
  /// Key is always true and can't be changed. Single entity is forced in database
  /// </summary>
  [Key]
  public bool Key {
    get => _key;
    set => _key = true;
  }

}