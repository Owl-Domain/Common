namespace OwlDomain.Common;

/// <summary>
///   Represents the base implementation for a disposable instance.
/// </summary>
public abstract class DisposableBase : IDisposable, IAsyncDisposable
{
   #region Properties
   /// <summary>Whether the instance has been disposed already.</summary>
   public bool IsDisposed { get; private set; }
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

   /// <summary>Disposes any resources used by the instance.</summary>
   /// <param name="deterministic">Whether managed resources can be disposed.</param>
   /// <remarks>If called from a finaliser, the <paramref name="deterministic"/> argument should be <see langword="false"/>.</remarks>
   protected void Dispose(bool deterministic)
   {
      if (IsDisposed)
         return;

      if (deterministic)
         DisposeManaged();

      DisposeUnmanaged();

      IsDisposed = true;
   }

   /// <summary>Disposes the managed resources.</summary>
   protected virtual void DisposeManaged() { }

   /// <summary>Disposes the unmanaged resources.</summary>
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
   /// <remarks>The default implementation will invoke the <see cref="DisposeManaged"/> method.</remarks>
   protected virtual ValueTask DisposeManagedAsync()
   {
      DisposeManaged();
      return default;
   }

   /// <summary>Disposes the unmanaged resources.</summary>
   /// <returns>A task representing the asynchronous operation.</returns>
   /// <remarks>The default implementation will invoke the <see cref="DisposeUnmanaged"/> method.</remarks>
   protected virtual ValueTask DisposeUnmanagedAsync()
   {
      DisposeUnmanaged();
      return default;
   }

   /// <summary>Throws the <see cref="ObjectDisposedException"/> if the instance has been disposed.</summary>
   protected void ThrowIfDisposed()
   {
      if (IsDisposed)
         Throw.For.ObjectDisposed(GetType().Name);
   }
   #endregion
}
