namespace EntityFrameworkCore.Extender;

/// <summary>
/// Initializes <see cref="EfCoreExtenderOptions"/> with default values.
///
/// Must be added as singleton to DI to be used by UnitOfWorkBase.
/// </summary>
public sealed class EfCoreExtenderOptions
{
   /// <summary>
   /// Enables default logging messages for database save actions. It uses Serilog.
   /// </summary>
   public bool EnableDefaultErrorLogging { get; set; } = true;
  
   /// <summary>
   /// Verifies save changes affected rows count if it is matching ChangeTracker.
   /// If ChangeTracker is disabled this will throw an error.
   /// <br/>
   /// If ChangeTracker changed entity count is not matching affected rows count save will not persist.
   /// </summary>
   public bool ValidateAffectedRows  { get; set; } = false;

   /// <summary>
   /// Whether to use Transactions for save operation.
   /// </summary>
   public bool UseTransactions { get; set; } = true;
}