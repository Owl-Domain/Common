namespace Common.Tests;

[TestClass]
public sealed class DisposableBaseTests
{
   #region Nested types
   private sealed class TestDisposable(Action disposeManagedCallback, Action disposeUnmanagedCallback) : DisposableBase
   {
      #region Fields
      private readonly Action _disposeManagedCallback = disposeManagedCallback;
      private readonly Action _disposeUnmanagedCallback = disposeUnmanagedCallback;
      #endregion

      #region Properties
      new public bool IsDisposed => base.IsDisposed;
      #endregion

      #region Methods
      protected override void DisposeManaged() => _disposeManagedCallback.Invoke();
      protected override void DisposeUnmanaged() => _disposeUnmanagedCallback.Invoke();
      #endregion
   }

   private sealed class AsyncTestDisposable(Action disposeManagedCallback, Action disposeUnmanagedCallback) : DisposableBase
   {
      #region Fields
      private readonly Action _disposeManagedCallback = disposeManagedCallback;
      private readonly Action _disposeUnmanagedCallback = disposeUnmanagedCallback;
      #endregion

      #region Properties
      new public bool IsDisposed => base.IsDisposed;
      #endregion

      #region Methods
      protected override ValueTask DisposeManagedAsync()
      {
         _disposeManagedCallback.Invoke();
         return default;
      }
      protected override ValueTask DisposeUnmanagedAsync()
      {
         _disposeUnmanagedCallback.Invoke();
         return default;
      }
      #endregion
   }
   #endregion

   #region Tests
   [TestMethod]
   public void Dispose_FirstCall_DisposeCallbacksInvokedOnce()
   {
      // Arrange
      int disposedManagedCount = 0;
      int disposedUnmanagedCount = 0;

      void DisposeManagedCallback() => disposedManagedCount++;
      void DisposeUnmanagedCallback() => disposedUnmanagedCount++;

      TestDisposable sut = new(DisposeManagedCallback, DisposeUnmanagedCallback);

      // Arrange assert
      Assert.IsConclusiveIf.IsFalse(sut.IsDisposed);

      // Act
      sut.Dispose();

      // Assert
      Assert.That
         .IsTrue(sut.IsDisposed)
         .AreEqual(disposedManagedCount, 1)
         .AreEqual(disposedUnmanagedCount, 1);
   }

   [TestMethod]
   public void Dispose_SecondCall_DisposeCallbacksInvokedOnce()
   {
      // Arrange
      int disposedManagedCount = 0;
      int disposedUnmanagedCount = 0;

      void DisposeManagedCallback() => disposedManagedCount++;
      void DisposeUnmanagedCallback() => disposedUnmanagedCount++;

      TestDisposable sut = new(DisposeManagedCallback, DisposeUnmanagedCallback);

      // Arrange assert
      Assert.IsConclusiveIf.IsFalse(sut.IsDisposed);

      // Act
      sut.Dispose();
      Assert.IsConclusiveIf.IsTrue(sut.IsDisposed);
      sut.Dispose();

      // Assert
      Assert.That
         .IsTrue(sut.IsDisposed)
         .AreEqual(disposedManagedCount, 1)
         .AreEqual(disposedUnmanagedCount, 1);
   }

   [TestMethod]
   public void Finalizer_NotDisposed_DisposesUnmanagedOnly()
   {
      // Arrange
      [MethodImpl(MethodImplOptions.NoInlining)]
      static void CreateSut(Action disposeManagedCallback, Action disposeUnmanagedCallback)
      {
         _ = new TestDisposable(disposeManagedCallback, disposeUnmanagedCallback);
      }
      int disposedManagedCount = 0;
      int disposedUnmanagedCount = 0;

      [ExcludeFromCodeCoverage(Justification = "Is required for checking if test will fail.")]
      void DisposeManagedCallback() => disposedManagedCount++;
      void DisposeUnmanagedCallback() => disposedUnmanagedCount++;

      CreateSut(DisposeManagedCallback, DisposeUnmanagedCallback);

      // Act
      GC.Collect();
      GC.WaitForPendingFinalizers();

      // Assert
      Assert.That
         .AreEqual(disposedManagedCount, 0)
         .AreEqual(disposedUnmanagedCount, 1);
   }

   [TestMethod]
   public async Task DisposeAsync_FirstCall_DisposeCallbacksInvokedOnce()
   {
      // Arrange
      int disposedManagedCount = 0;
      int disposedUnmanagedCount = 0;

      void DisposeManagedCallback() => disposedManagedCount++;
      void DisposeUnmanagedCallback() => disposedUnmanagedCount++;

      AsyncTestDisposable sut = new(DisposeManagedCallback, DisposeUnmanagedCallback);

      // Arrange assert
      Assert.IsConclusiveIf.IsFalse(sut.IsDisposed);

      // Act
      await sut.DisposeAsync();

      // Assert
      Assert.That
         .IsTrue(sut.IsDisposed)
         .AreEqual(disposedManagedCount, 1)
         .AreEqual(disposedUnmanagedCount, 1);
   }

   [TestMethod]
   public async Task DisposeAsync_SecondCall_DisposeCallbacksInvokedOnce()
   {
      // Arrange
      int disposedManagedCount = 0;
      int disposedUnmanagedCount = 0;

      void DisposeManagedCallback() => disposedManagedCount++;
      void DisposeUnmanagedCallback() => disposedUnmanagedCount++;

      AsyncTestDisposable sut = new(DisposeManagedCallback, DisposeUnmanagedCallback);

      // Arrange assert
      Assert.IsConclusiveIf.IsFalse(sut.IsDisposed);

      // Act
      await sut.DisposeAsync();
      Assert.IsConclusiveIf.IsTrue(sut.IsDisposed);
      await sut.DisposeAsync();

      // Assert
      Assert.That
         .IsTrue(sut.IsDisposed)
         .AreEqual(disposedManagedCount, 1)
         .AreEqual(disposedUnmanagedCount, 1);
   }

   [TestMethod]
   public async Task DisposeAsync_FirstCall_RedirectsToAsyncDisposeCallbacks()
   {
      // Arrange
      int disposedManagedCount = 0;
      int disposedUnmanagedCount = 0;

      void DisposeManagedCallback() => disposedManagedCount++;
      void DisposeUnmanagedCallback() => disposedUnmanagedCount++;

      TestDisposable sut = new(DisposeManagedCallback, DisposeUnmanagedCallback);

      // Arrange assert
      Assert.IsConclusiveIf.IsFalse(sut.IsDisposed);

      // Act
      await sut.DisposeAsync();

      // Assert
      Assert.That
         .IsTrue(sut.IsDisposed)
         .AreEqual(disposedManagedCount, 1)
         .AreEqual(disposedUnmanagedCount, 1);
   }

   [TestMethod]
   public async Task DisposeAsync_SecondCall_RedirectsToAsyncDisposeCallbacks()
   {
      // Arrange
      int disposedManagedCount = 0;
      int disposedUnmanagedCount = 0;

      void DisposeManagedCallback() => disposedManagedCount++;
      void DisposeUnmanagedCallback() => disposedUnmanagedCount++;

      TestDisposable sut = new(DisposeManagedCallback, DisposeUnmanagedCallback);

      // Arrange assert
      Assert.IsConclusiveIf.IsFalse(sut.IsDisposed);

      // Act
      await sut.DisposeAsync();
      Assert.IsConclusiveIf.IsTrue(sut.IsDisposed);
      await sut.DisposeAsync();

      // Assert
      Assert.That
         .IsTrue(sut.IsDisposed)
         .AreEqual(disposedManagedCount, 1)
         .AreEqual(disposedUnmanagedCount, 1);
   }
   #endregion
}
