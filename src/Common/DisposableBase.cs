namespace OwlDomain.Common;

/// <summary>
///   Represents the base implementation for a disposable instance.
/// </summary>
public abstract class DisposableBase : IDisposable, IAsyncDisposable
{
   #region Properties
   /// <summary>Whether the instance has been disposed already.</summary>
   protected bool IsDisposed { get; private set; }
   #endregion

   #region Constructors
   /// <summary>Disposes the instance.</summary>
   ~DisposableBase() => Dispose(false);
   #endregion

   #region Methods
   /// <inheritdoc/>
   public void Dispose()
   {
      Dispose(true);
      GC.SuppressFinalize(this);
   }

   private void Dispose(bool deterministic)
   {
      if (IsDisposed)
         return;

      if (deterministic)
         DisposeManaged();

      DisposeUnmanaged();

      IsDisposed = true;
   }

   /// <summary>Disposes the managed resources.</summary>
   [ExcludeFromCodeCoverage]
   protected virtual void DisposeManaged() { }

   /// <summary>Disposes the unmanaged resources.</summary>
   [ExcludeFromCodeCoverage]
   protected virtual void DisposeUnmanaged() { }

   /// <inheritdoc/>
   public async ValueTask DisposeAsync()
   {
      if (IsDisposed)
         return;

      await DisposeManagedAsync();
      await DisposeUnmanagedAsync();

      IsDisposed = true;

      GC.SuppressFinalize(this);
   }

   /// <summary>Disposes the managed resources.</summary>
   /// <returns>A task representing the asynchronous operation.</returns>
   protected virtual ValueTask DisposeManagedAsync()
   {
      DisposeManaged();
      return default;
   }

   /// <summary>Disposes the unmanaged resources.</summary>
   /// <returns>A task representing the asynchronous operation.</returns>
   protected virtual ValueTask DisposeUnmanagedAsync()
   {
      DisposeUnmanaged();
      return default;
   }
   #endregion
}
