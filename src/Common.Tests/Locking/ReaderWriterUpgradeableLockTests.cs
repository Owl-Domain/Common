namespace Common.Tests.Locking;

[TestClass]
public sealed class ReaderWriterUpgradeableLockTests
{
   #region Tests
   [TestMethod]
   public void Ctor_SetsExpectedFields()
   {
      // Arrange
      ReaderWriterLockSlim @lock = new();

      // Act
      ReaderWriterUpgradeableLock sut = new(@lock);

      // Assert
      Assert.That.AreSameInstance(sut.ReaderWriterLock, @lock);
   }

   [TestMethod]
   public void Dispose_WithAcquiredLock_ReleasesLock()
   {
      // Arrange
      ReaderWriterLockSlim @lock = new();
      ReaderWriterUpgradeableLock sut = new(@lock);

      @lock.EnterUpgradeableReadLock();

      // Arrange assert
      Assert.IsConclusiveIf.IsTrue(@lock.IsUpgradeableReadLockHeld);

      // Act
      sut.Dispose();

      // Assert
      Assert.That.IsFalse(@lock.IsUpgradeableReadLockHeld);
   }

   [TestMethod]
   public void Dispose_WithoutAcquiredLock_ThrowsSynchronizationLockException()
   {
      // Arrange
      ReaderWriterLockSlim @lock = new();
      ReaderWriterUpgradeableLock sut = new(@lock);

      // Arrange assert
      Assert.IsConclusiveIf.IsFalse(@lock.IsUpgradeableReadLockHeld);

      // Act
      void Act() => sut.Dispose();

      // Assert
      Assert.That.ThrowsExactException<SynchronizationLockException>(Act);
   }
   #endregion
}
