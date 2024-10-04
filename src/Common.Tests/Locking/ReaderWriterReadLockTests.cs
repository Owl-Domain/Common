namespace Common.Tests.Locking;

[TestClass]
public sealed class ReaderWriterReadLockTests
{
   #region Tests
   [TestMethod]
   public void Ctor_SetsExpectedFields()
   {
      // Arrange
      ReaderWriterLockSlim @lock = new();

      // Act
      ReaderWriterReadLock sut = new(@lock);

      // Assert
      Assert.That.AreSameInstance(sut.ReaderWriterLock, @lock);
   }

   [TestMethod]
   public void Dispose_WithAcquiredLock_ReleasesLock()
   {
      // Arrange
      ReaderWriterLockSlim @lock = new();
      ReaderWriterReadLock sut = new(@lock);

      @lock.EnterReadLock();

      // Arrange assert
      Assert.IsConclusiveIf.IsTrue(@lock.IsReadLockHeld);

      // Act
      sut.Dispose();

      // Assert
      Assert.That.IsFalse(@lock.IsReadLockHeld);
   }

   [TestMethod]
   public void Dispose_WithoutAcquiredLock_ThrowsSynchronizationLockException()
   {
      // Arrange
      ReaderWriterLockSlim @lock = new();
      ReaderWriterReadLock sut = new(@lock);

      // Arrange assert
      Assert.IsConclusiveIf.IsFalse(@lock.IsReadLockHeld);

      // Act
      void Act() => sut.Dispose();

      // Assert
      Assert.That.ThrowsExactException<SynchronizationLockException>(Act);
   }
   #endregion
}
