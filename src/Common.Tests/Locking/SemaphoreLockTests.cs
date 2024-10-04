namespace Common.Tests.Locking;

[TestClass]
public sealed class SemaphoreLockTests
{
   #region Tests
   [TestMethod]
   public void Ctor_WithValidReleaseCount_NoExceptionIsThrown()
   {
      // Arrange
      SemaphoreSlim semaphore = new(1);
      const int releaseCount = 1;

      // Act
      SemaphoreLock Act() => new(semaphore, releaseCount);

      // Assert
      Assert.That
         .DoesNotThrowAnyException(Act, out SemaphoreLock sut)
         .AreSameInstance(sut.Semaphore, semaphore)
         .AreEqual(sut.ReleaseCount, releaseCount);
   }

   [DataRow(0, DisplayName = "Zero")]
   [DataRow(-1, DisplayName = "Below Zero")]
   [TestMethod]
   public void Ctor_WithInvalidReleaseCount_ThrowsArgumentOutOfRangeException(int releaseCount)
   {
      // Arrange
      const string expectedArgumentName = "releaseCount";
      SemaphoreSlim semaphore = new(1);

      // Act
      void Act() => _ = new SemaphoreLock(semaphore, releaseCount);

      // Assert
      Assert.That
         .ThrowsExactException(Act, out ArgumentOutOfRangeException exception)
         .AreEqual(exception.ParamName, expectedArgumentName);
   }

   [TestMethod]
   public void Ctor_WithNoReleaseCount_NoExceptionIsThrown()
   {
      // Arrange
      const int expectedReleaseCount = 1;
      SemaphoreSlim semaphore = new(1);

      // Act
      SemaphoreLock Act() => new(semaphore);

      // Assert
      Assert.That
         .DoesNotThrowAnyException(Act, out SemaphoreLock sut)
         .AreSameInstance(sut.Semaphore, semaphore)
         .AreEqual(sut.ReleaseCount, expectedReleaseCount);
   }

   [TestMethod]
   public void Dispose_IncreasesCount()
   {
      // Arrange
      const int initialCount = 1;

      SemaphoreSlim semaphore = new(initialCount);
      SemaphoreLock sut = new(semaphore);

      // Act
      sut.Dispose();

      // Assert
      Assert.That
         .AreEqual(semaphore.CurrentCount, initialCount + 1)
         .AreSameInstance(sut.Semaphore, semaphore);
   }
   #endregion
}
