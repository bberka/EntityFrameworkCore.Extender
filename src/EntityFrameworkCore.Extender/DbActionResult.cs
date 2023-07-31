namespace EntityFrameworkCore.Extender;

public record DbActionResult(bool Status, bool IsRollback, int AffectedRows, Exception? Exception);