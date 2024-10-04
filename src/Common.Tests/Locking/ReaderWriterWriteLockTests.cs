namespace Common.Tests.Locking;

[TestClass]
public sealed class ReaderWriterWriteLockTests
{
   #region Tests
   [TestMethod]
   public void Ctor_SetsExpectedFields()
   {
      // Arrange
      ReaderWriterLockSlim @lock = new();

      // Act
      ReaderWriterWriteLock sut = new(@lock);

      // Assert
      Assert.That.AreSameInstance(sut.ReaderWriterLock, @lock);
   }

   [TestMethod]
   public void Dispose_WithAcquiredLock_ReleasesLock()
   {
      // Arrange
      ReaderWriterLockSlim @lock = new();
      ReaderWriterWriteLock sut = new(@lock);

      @lock.EnterWriteLock();

      // Arrange assert
      Assert.IsConclusiveIf.IsTrue(@lock.IsWriteLockHeld);

      // Act
      sut.Dispose();

      // Assert
      Assert.That.IsFalse(@lock.IsWriteLockHeld);
   }

   [TestMethod]
   public void Dispose_WithoutAcquiredLock_ThrowsSynchronizationLockException()
   {
      // Arrange
      ReaderWriterLockSlim @lock = new();
      ReaderWriterWriteLock sut = new(@lock);

      // Arrange assert
      Assert.IsConclusiveIf.IsFalse(@lock.IsWriteLockHeld);

      // Act
      void Act() => sut.Dispose();

      // Assert
      Assert.That.ThrowsExactException<SynchronizationLockException>(Act);
   }
   #endregion
}
