using System.Transactions;
using EntityFrameworkCore.Extender.Abstract;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Serilog;

namespace EntityFrameworkCore.Extender;

public abstract class UnitOfWorkBase<TContext> : IUnitOfWorkBase
   where TContext : DbContext
{
   protected readonly TContext DbContext;
   private readonly EfCoreExtenderOptions _options;

   protected UnitOfWorkBase(TContext dbContext, EfCoreExtenderOptions? options = null)
   {
      DbContext = dbContext;
      _options = options ?? new();
      IsTransactionBegan = false;
   }

   
   protected bool IsDisposed { get; private set; }
   protected IDbContextTransaction? Transaction { get; private set; }
   protected bool IsTransactionBegan { get; private set; }


   public virtual DbActionResult SaveResult()
   {
      try {
         BeginTransaction();
         var changedEntryCount = GetChangedEntryCount();
         if (changedEntryCount == 0 && _options.ValidateAffectedRows) {
            if (_options.EnableDefaultErrorLogging)
               Log.Debug("Db save failed: no changes");
            return new DbActionResult(false, false, 0, null);
         }

         var affectedRows = DbContext.SaveChanges();
         if (affectedRows == 0) {
            RollbackTransaction();
            if (_options.EnableDefaultErrorLogging)
               Log.Debug("Db save failed: affected rows 0");
            return new DbActionResult(false, true, 0, null);
         }

         var match = affectedRows == changedEntryCount;
         if (_options.ValidateAffectedRows && !match) {
            RollbackTransaction();
            if (_options.EnableDefaultErrorLogging)
               Log.Error(
                  "Db save failed: affected rows not matching changed entry count in ChangeTracker. Affected rows: {affectedRows}, Changed entry count: {changedEntryCount}",
                  affectedRows, changedEntryCount);
            return new DbActionResult(false, true, 0, null);
         }

         CommitTransaction();
         if (_options.EnableDefaultErrorLogging)
            Log.Debug("Db saved successfully");
         return new DbActionResult(true, false, affectedRows, null);
      }
      catch (Exception ex) {
         RollbackTransaction();
         if (_options.EnableDefaultErrorLogging)
            Log.Fatal(ex, "InternalDbError");
         return new DbActionResult(false, true, 0, ex);
      }
      finally {
         ResetTransaction();
      }
   }

   public virtual async Task<DbActionResult> SaveResultAsync()
   {
      try {
         await BeginTransactionAsync();
         var changedEntryCount = GetChangedEntryCount();
         if (changedEntryCount == 0) {
            if (_options.EnableDefaultErrorLogging)
               Log.Debug("Db save failed: no changes");
            return new DbActionResult(false, false, 0, null);
         }

         var affectedRows = await DbContext.SaveChangesAsync();
         if (affectedRows == 0) {
            if (_options.EnableDefaultErrorLogging)
               Log.Debug("Db save failed: affected rows 0");
            await RollbackTransactionAsync();
            return new DbActionResult(false, true, 0, null);
         }

         var match = affectedRows == changedEntryCount;
         if (_options.ValidateAffectedRows && !match) {
            await RollbackTransactionAsync();

            if (_options.EnableDefaultErrorLogging)
               Log.Error(
                  "Db save failed: affected rows not matching changed entry count in ChangeTracker. Affected rows: {affectedRows}, Changed entry count: {changedEntryCount}",
                  affectedRows, changedEntryCount);
            return new DbActionResult(false, true, 0, null);
         }

         if (_options.EnableDefaultErrorLogging)
            Log.Debug("Db saved successfully");
         await CommitTransactionAsync();

         return new DbActionResult(true, false, affectedRows, null);
      }
      catch (Exception ex) {
         if (_options.EnableDefaultErrorLogging)
            Log.Fatal(ex, "InternalDbError");
         await RollbackTransactionAsync();
         return new DbActionResult(false, true, 0, ex);
      }
      finally {
         ResetTransaction();
      }
   }


   public virtual bool Save()
   {
      var result = SaveResult();
      return result.Status;
   }

   public virtual async Task<bool> SaveAsync()
   {
      var result = await SaveResultAsync();
      return result.Status;
   }


   public virtual int SaveChanges() => SaveResult().AffectedRows;

   public virtual async Task<int> SaveChangesAsync() => (await SaveResultAsync()).AffectedRows;

   public bool HasChanges()
   {
      if (!DbContext.ChangeTracker.AutoDetectChangesEnabled)
         throw new InvalidOperationException("DbSave can not be called if auto detect changes is off");
      return DbContext.ChangeTracker.HasChanges();
   }
   public int GetChangedEntryCount()
   {
      if (!DbContext.ChangeTracker.AutoDetectChangesEnabled)
         throw new InvalidOperationException("DbSave can not be called if auto detect changes is off");
      var changes = DbContext.ChangeTracker
         .Entries()
         .Count(x => x.State != EntityState.Unchanged && x.State != EntityState.Detached);
      return changes;
   }
   public void Dispose()
   {
      IsDisposed = true;
      DbContext.Dispose();
      Transaction?.Dispose();
      GC.SuppressFinalize(this);
   }


   protected void BeginTransaction()
   {
      if(!_options.UseTransactions) return;
      if (IsTransactionBegan) throw new TransactionException("Transaction already began");
      IsTransactionBegan = true;
      Transaction = DbContext.Database.BeginTransaction();
   }

   protected async Task BeginTransactionAsync()
   {
      if(!_options.UseTransactions) return;
      if (IsTransactionBegan) throw new TransactionException("Transaction already began");
      IsTransactionBegan = true;
      Transaction = await DbContext.Database.BeginTransactionAsync();
   }


   protected void ResetTransaction()
   {
      if(!_options.UseTransactions) return;
      if (!IsTransactionBegan) return;
      Transaction = null;
      IsTransactionBegan = false;
   }
   
   private bool CommitTransaction()
   {
      if(!_options.UseTransactions) return true;
      if (!IsTransactionBegan) 
         throw new TransactionException(nameof(CommitTransaction) + " called without transaction");
      Transaction!.Commit();
      return true;
   }
   
   private async Task<bool> CommitTransactionAsync()
   {
      if(!_options.UseTransactions) return true;
      if (!IsTransactionBegan)
         throw new TransactionException(nameof(CommitTransactionAsync) + " called without transaction");

      await Transaction!.CommitAsync();
      return true;
   }
   
   private bool RollbackTransaction()
   {
      if(!_options.UseTransactions) return true;
      if (!IsTransactionBegan) 
         throw new TransactionException(nameof(RollbackTransaction) + " called without transaction");
      Transaction!.Rollback();
      return true;
   }
   
   private async Task<bool> RollbackTransactionAsync()
   {
      if(!_options.UseTransactions) return true;
      if (!IsTransactionBegan) 
         throw new TransactionException(nameof(RollbackTransactionAsync) + " called without transaction");
      await Transaction!.RollbackAsync();
      return true;
   }
   
 
}
