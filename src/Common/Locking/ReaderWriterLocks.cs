namespace OwlDomain.Common.Locking;

/// <summary>
///   Represents a read lock around a <see cref="ReaderWriterLockSlim"/>.
/// </summary>
/// <param name="readerWriterLock">The reader/writer lock to hold in a read lock.</param>
public readonly struct ReaderWriterReadLock(ReaderWriterLockSlim readerWriterLock) : IDisposable
{
   #region Fields
   /// <summary>The reader/writer lock to hold in a read lock.</summary>
   public readonly ReaderWriterLockSlim ReaderWriterLock = readerWriterLock;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public readonly void Dispose() => ReaderWriterLock.ExitReadLock();
   #endregion
}

/// <summary>
///   Represents a write lock around a <see cref="ReaderWriterLockSlim"/>.
/// </summary>
/// <param name="readerWriterLock">The reader/writer lock to hold in a write lock.</param>
public readonly struct ReaderWriterWriteLock(ReaderWriterLockSlim readerWriterLock) : IDisposable
{
   #region Fields
   /// <summary>The reader/writer lock to hold in a write lock.</summary>
   public readonly ReaderWriterLockSlim ReaderWriterLock = readerWriterLock;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public readonly void Dispose() => ReaderWriterLock.ExitWriteLock();
   #endregion
}

/// <summary>
///   Represents an upgradeable read lock around a <see cref="ReaderWriterLockSlim"/>.
/// </summary>
/// <param name="readerWriterLock">The reader/writer lock to hold in an upgradeable read lock.</param>
public readonly struct ReaderWriterUpgradeableLock(ReaderWriterLockSlim readerWriterLock) : IDisposable
{
   #region Fields
   /// <summary>The reader/writer lock to hold in an upgradeable read lock.</summary>
   public readonly ReaderWriterLockSlim ReaderWriterLock = readerWriterLock;
   #endregion

   #region Methods
   /// <inheritdoc/>
   public readonly void Dispose() => ReaderWriterLock.ExitUpgradeableReadLock();
   #endregion
}

/// <summary>
///   Contains various extension methods related to the different <see cref="ReaderWriterLockSlim"/> locks.
/// </summary>
public static class ReaderWriterLockExtensions
{
   #region Methods
   /// <summary>
   ///   Puts the given <paramref name="readerWriterLock"/>
   ///   into a read lock until the return value is disposed.
   /// </summary>
   /// <param name="readerWriterLock">The reader writer lock to put in a read lock.</param>
   /// <returns>
   ///   A read lock around the given <paramref name="readerWriterLock"/>
   ///   which will be released when it is disposed.
   /// </returns>
   public static ReaderWriterReadLock ReadLock(this ReaderWriterLockSlim readerWriterLock)
   {
      readerWriterLock.EnterReadLock();
      return new(readerWriterLock);
   }

   /// <summary>
   ///   Puts the given <paramref name="readerWriterLock"/>
   ///   into a write lock until the return value is disposed.
   /// </summary>
   /// <param name="readerWriterLock">The reader writer lock to put in a write lock.</param>
   /// <returns>
   ///   A write lock around the given <paramref name="readerWriterLock"/>
   ///   which will be released when it is disposed.
   /// </returns>
   public static ReaderWriterWriteLock WriteLock(this ReaderWriterLockSlim readerWriterLock)
   {
      readerWriterLock.EnterWriteLock();
      return new(readerWriterLock);
   }

   /// <summary>
   ///   Puts the given <paramref name="readerWriterLock"/>
   ///   into an upgradeable read lock until the return value is disposed.
   /// </summary>
   /// <param name="readerWriterLock">The reader writer lock to put in an upgradeable read lock.</param>
   /// <returns>
   ///   An upgradeable read lock around the given <paramref name="readerWriterLock"/>
   ///   which will be released when it is disposed.
   /// </returns>
   public static ReaderWriterUpgradeableLock UpgradeableReadLock(this ReaderWriterLockSlim readerWriterLock)
   {
      readerWriterLock.EnterUpgradeableReadLock();
      return new(readerWriterLock);
   }
   #endregion
}
