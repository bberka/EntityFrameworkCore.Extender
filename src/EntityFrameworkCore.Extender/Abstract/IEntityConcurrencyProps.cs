using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EntityFrameworkCore.Extender.Abstract;

public interface IEntityConcurrencyProps
{
  [Timestamp]
  [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
  public byte[] Version { get; set; }
}