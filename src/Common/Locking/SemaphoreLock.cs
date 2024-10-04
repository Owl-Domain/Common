namespace OwlDomain.Common.Locking;

/// <summary>
///   Represents a lock around a <see cref="SemaphoreSlim"/>.
/// </summary>
public readonly struct SemaphoreLock : IDisposable
{
   #region Fields
   /// <summary>The semaphore that is locked.</summary>
   public readonly SemaphoreSlim Semaphore;

   /// <summary>The amount of times the <see cref="Semaphore"/> will be released.</summary>
   public readonly int ReleaseCount;
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="SemaphoreLock"/>.</summary>
   /// <param name="semaphore">The semaphore to lock.</param>
   /// <param name="releaseCount">
   ///   The amount of times the given <paramref name="semaphore"/>
   ///   should be released when the lock is ended.
   /// </param>
   public SemaphoreLock(SemaphoreSlim semaphore, int releaseCount)
   {
      Throw.IfArgument.IsTooLow(releaseCount, 1);

      Semaphore = semaphore;
      ReleaseCount = releaseCount;
   }

   /// <summary>Creates a new instance of the <see cref="SemaphoreLock"/>.</summary>
   /// <param name="semaphore">The semaphore to lock.</param>
   public SemaphoreLock(SemaphoreSlim semaphore)
   {
      Semaphore = semaphore;
      ReleaseCount = 1;
   }
   #endregion

   #region Methods
   /// <inheritdoc/>
   public readonly void Dispose() => Semaphore.Release(ReleaseCount);
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="SemaphoreLock"/>.
/// </summary>
public static class SemaphoreLockExtensions
{
   #region Methods
   /// <summary>
   ///   Locks the given <paramref name="semaphore"/>, and
   ///   releases it when the returned value is disposed.
   /// </summary>
   /// <param name="semaphore">The semaphore to lock.</param>
   /// <param name="cancellationToken">A token which can be used to cancel the operation.</param>
   /// <returns>A lock around the given <paramref name="semaphore"/> which will release it when disposed.</returns>
   /// <exception cref="OperationCanceledException">Thrown if the operation is cancelled.</exception>
   public static SemaphoreLock Lock(this SemaphoreSlim semaphore, CancellationToken cancellationToken = default)
   {
      cancellationToken.ThrowIfCancellationRequested();

      semaphore.Wait(cancellationToken);

      return new(semaphore);
   }

   /// <summary>
   ///   Locks the given <paramref name="semaphore"/>, and
   ///   releases it when the returned value is disposed.
   /// </summary>
   /// <param name="semaphore">The semaphore to lock.</param>
   /// <param name="releaseCount">
   ///   The amount of times the given <paramref name="semaphore"/>
   ///   should be released when the returned value is disposed.
   /// </param>
   /// <param name="cancellationToken">A token which can be used to cancel the operation.</param>
   /// <returns>A lock around the given <paramref name="semaphore"/> which will release it when disposed.</returns>
   /// <exception cref="OperationCanceledException">Thrown if the operation is cancelled.</exception>
   public static SemaphoreLock Lock(this SemaphoreSlim semaphore, int releaseCount, CancellationToken cancellationToken = default)
   {
      cancellationToken.ThrowIfCancellationRequested();

      semaphore.Wait(cancellationToken);

      return new(semaphore, releaseCount);
   }

   /// <summary>
   ///   Locks the given <paramref name="semaphore"/>, and
   ///   releases it when the returned value is disposed.
   /// </summary>
   /// <param name="semaphore">The semaphore to lock.</param>
   /// <param name="cancellationToken">A token which can be used to cancel the operation.</param>
   /// <returns>A lock around the given <paramref name="semaphore"/> which will release it when disposed.</returns>
   /// <exception cref="OperationCanceledException">Thrown if the operation is cancelled.</exception>
   public static async ValueTask<SemaphoreLock> LockAsync(this SemaphoreSlim semaphore, CancellationToken cancellationToken = default)
   {
      cancellationToken.ThrowIfCancellationRequested();

      await semaphore
         .WaitAsync(cancellationToken)
         .ConfigureAwait(false);

      return new(semaphore);
   }

   /// <summary>
   ///   Locks the given <paramref name="semaphore"/>, and
   ///   releases it when the returned value is disposed.
   /// </summary>
   /// <param name="semaphore">The semaphore to lock.</param>
   /// <param name="releaseCount">
   ///   The amount of times the given <paramref name="semaphore"/>
   ///   should be released when the returned value is disposed.
   /// </param>
   /// <param name="cancellationToken">A token which can be used to cancel the operation.</param>
   /// <returns>A lock around the given <paramref name="semaphore"/> which will release it when disposed.</returns>
   /// <exception cref="OperationCanceledException">Thrown if the operation is cancelled.</exception>
   public static async ValueTask<SemaphoreLock> LockAsync(this SemaphoreSlim semaphore, int releaseCount, CancellationToken cancellationToken = default)
   {
      cancellationToken.ThrowIfCancellationRequested();

      await semaphore
         .WaitAsync(cancellationToken)
         .ConfigureAwait(false);

      return new(semaphore, releaseCount);
   }
   #endregion
}
