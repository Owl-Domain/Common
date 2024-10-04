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

      #region Methods
      protected override void DisposeManaged()
      {
         base.DisposeManaged();
         _disposeManagedCallback.Invoke();
      }
      protected override void DisposeUnmanaged()
      {
         base.DisposeUnmanaged();
         _disposeUnmanagedCallback.Invoke();
      }
      #endregion
   }
   private sealed class AsyncTestDisposable(Action disposeManagedCallback, Action disposeUnmanagedCallback) : DisposableBase
   {
      #region Fields
      private readonly Action _disposeManagedCallback = disposeManagedCallback;
      private readonly Action _disposeUnmanagedCallback = disposeUnmanagedCallback;
      #endregion

      #region Methods
      protected override async ValueTask DisposeManagedAsync()
      {
         await base.DisposeManagedAsync();
         _disposeManagedCallback.Invoke();
      }
      protected override async ValueTask DisposeUnmanagedAsync()
      {
         await base.DisposeUnmanagedAsync();
         _disposeUnmanagedCallback.Invoke();
      }
      #endregion
   }
   private sealed class SimpleTestDisposable : DisposableBase
   {
      #region Methods
      new public void ThrowIfDisposed() => base.ThrowIfDisposed();
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

   [TestMethod]
   public void ThrowIfDisposed_NotDisposed_DoesNotThrowAntException()
   {
      // Arrange
      SimpleTestDisposable sut = new();

      // Act
      void Act() => sut.ThrowIfDisposed();

      // Assert
      Assert.That.DoesNotThrowAnyException(Act);
   }

   [TestMethod]
   public void ThrowIfDisposed_IsDisposed_ThrowsObjectDisposedException()
   {
      // Arrange
      const string expectedName = nameof(SimpleTestDisposable);

      SimpleTestDisposable sut = new();
      sut.Dispose();

      // Act
      void Act() => sut.ThrowIfDisposed();

      // Assert
      Assert.That
         .ThrowsExactException(Act, out ObjectDisposedException exception)
         .AreEqual(exception.ObjectName, expectedName);
   }
   #endregion
}
