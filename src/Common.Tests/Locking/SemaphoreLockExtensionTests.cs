namespace Common.Tests.Locking;

[TestClass]
public sealed class SemaphoreLockExtensionTests
{
   #region Lock tests
   [TestMethod]
   public void Lock_WithCancelledToken_ThrowsOperationCancelledException()
   {
      // Arrange
      SemaphoreSlim semaphore = new(1);
      CancellationToken token = new(true);

      // Act
      void Act() => _ = SemaphoreLockExtensions.Lock(semaphore, token);

      // Assert
      Assert.That.ThrowsExactException<OperationCanceledException>(Act);
   }

   [TestMethod]
   public void Lock_AcquiresLockAndExpectedReleaseCount()
   {
      // Arrange
      SemaphoreSlim semaphore = new(1);

      // Arrange assert
      Assert.IsConclusiveIf.AreEqual(semaphore.CurrentCount, 1);

      // Act
      SemaphoreLock @lock = SemaphoreLockExtensions.Lock(semaphore);

      // Assert
      Assert.That
         .AreEqual(semaphore.CurrentCount, 0)
         .AreSameInstance(@lock.Semaphore, semaphore)
         .AreEqual(@lock.ReleaseCount, 1);
   }

   [TestMethod]
   public void Lock_WithReleaseCount_WithCancelledToken_ThrowsOperationCancelledException()
   {
      // Arrange
      SemaphoreSlim semaphore = new(1);
      CancellationToken token = new(true);

      // Act
      void Act() => _ = SemaphoreLockExtensions.Lock(semaphore, 2, token);

      // Assert
      Assert.That.ThrowsExactException<OperationCanceledException>(Act);
   }

   [TestMethod]
   public void Lock_WithReleaseCount_AcquiresLockAndExpectedReleaseCount()
   {
      // Arrange
      const int releaseCount = 2;
      SemaphoreSlim semaphore = new(releaseCount);

      // Arrange assert
      Assert.IsConclusiveIf.AreEqual(semaphore.CurrentCount, releaseCount);

      // Act
      SemaphoreLock @lock = SemaphoreLockExtensions.Lock(semaphore, releaseCount);

      // Assert
      Assert.That
         .AreEqual(semaphore.CurrentCount, releaseCount - 1)
         .AreSameInstance(@lock.Semaphore, semaphore)
         .AreEqual(@lock.ReleaseCount, releaseCount);
   }
   #endregion

   #region LockAsync tests
   [TestMethod]
   public async Task LockAsync_WithCancelledToken_ThrowsOperationCancelledException()
   {
      // Arrange
      SemaphoreSlim semaphore = new(1);
      CancellationToken token = new(true);

      // Act
      async ValueTask Act() => _ = await SemaphoreLockExtensions.LockAsync(semaphore, token);

      // Assert
      await Assert.That.ThrowsExactExceptionAsync<OperationCanceledException>(Act);
   }

   [TestMethod]
   public async Task LockAsync_AcquiresLockAndExpectedReleaseCount()
   {
      // Arrange
      SemaphoreSlim semaphore = new(1);

      // Arrange assert
      Assert.IsConclusiveIf.AreEqual(semaphore.CurrentCount, 1);

      // Act
      SemaphoreLock @lock = await SemaphoreLockExtensions.LockAsync(semaphore);

      // Assert
      Assert.That
         .AreEqual(semaphore.CurrentCount, 0)
         .AreSameInstance(@lock.Semaphore, semaphore)
         .AreEqual(@lock.ReleaseCount, 1);
   }

   [TestMethod]
   public async Task LockAsync_WithReleaseCount_WithCancelledToken_ThrowsOperationCancelledException()
   {
      // Arrange
      SemaphoreSlim semaphore = new(1);
      CancellationToken token = new(true);

      // Act
      async ValueTask Act() => _ = await SemaphoreLockExtensions.LockAsync(semaphore, 2, token);

      // Assert
      await Assert.That.ThrowsExactExceptionAsync<OperationCanceledException>(Act);
   }

   [TestMethod]
   public async Task LockAsync_WithReleaseCount_AcquiresLockAndExpectedReleaseCount()
   {
      // Arrange
      const int releaseCount = 2;
      SemaphoreSlim semaphore = new(releaseCount);

      // Arrange assert
      Assert.IsConclusiveIf.AreEqual(semaphore.CurrentCount, releaseCount);

      // Act
      SemaphoreLock @lock = await SemaphoreLockExtensions.LockAsync(semaphore, releaseCount);

      // Assert
      Assert.That
         .AreEqual(semaphore.CurrentCount, releaseCount - 1)
         .AreSameInstance(@lock.Semaphore, semaphore)
         .AreEqual(@lock.ReleaseCount, releaseCount);
   }
   #endregion
}
