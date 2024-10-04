namespace Common.Tests.Locking;

[TestClass]
public sealed class ReaderWriterLockExtensionTests
{
   #region Tests
   [TestMethod]
   public void ReadLock_WithoutLock_AcquiresReadLock()
   {
      // Arrange
      ReaderWriterLockSlim @lock = new();

      // Arrange assert
      Assert.IsConclusiveIf.IsFalse(@lock.IsReadLockHeld);

      // Act
      ReaderWriterReadLock readLock = ReaderWriterLockExtensions.ReadLock(@lock);

      // Arrange
      Assert.That
         .AreSameInstance(readLock.ReaderWriterLock, @lock)
         .IsTrue(@lock.IsReadLockHeld);
   }

   [TestMethod]
   public void WriteLock_WithoutLock_AcquiresWriteLock()
   {
      // Arrange
      ReaderWriterLockSlim @lock = new();

      // Arrange assert
      Assert.IsConclusiveIf.IsFalse(@lock.IsWriteLockHeld);

      // Act
      ReaderWriterWriteLock writeLock = ReaderWriterLockExtensions.WriteLock(@lock);

      // Arrange
      Assert.That
         .AreSameInstance(writeLock.ReaderWriterLock, @lock)
         .IsTrue(@lock.IsWriteLockHeld);
   }

   [TestMethod]
   public void UpgradeableReadLock_WithoutLock_AcquiresUpgradeableReadLock()
   {
      // Arrange
      ReaderWriterLockSlim @lock = new();

      // Arrange assert
      Assert.IsConclusiveIf.IsFalse(@lock.IsUpgradeableReadLockHeld);

      // Act
      ReaderWriterUpgradeableLock readLock = ReaderWriterLockExtensions.UpgradeableReadLock(@lock);

      // Arrange
      Assert.That
         .AreSameInstance(readLock.ReaderWriterLock, @lock)
         .IsTrue(@lock.IsUpgradeableReadLockHeld);
   }
   #endregion
}
