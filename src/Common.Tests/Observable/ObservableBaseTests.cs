using System.ComponentModel;

namespace Common.Tests.Observable;

[TestClass]
public sealed class ObservableBaseTests
{
   #region Nested types
   [ExcludeFromCodeCoverage]
   private sealed class TestObservable : ObservableBase
   {
      #region Properties
      [Notifies(nameof(B))]
      public int A { get; }
      public int B { get; }
      #endregion

      #region Methods
      new public void RaisePropertyChanged(string? propertyName) => base.RaisePropertyChanged(propertyName);
      new public void RaisePropertyChanging(string? propertyName) => base.RaisePropertyChanging(propertyName);
      new public bool TrySetNoNotification<T>(ref T field, T value) => base.TrySetNoNotification(ref field, value);
      new public bool TrySet<T>(ref T field, T value, string propertyName) => base.TrySet(ref field, value, propertyName);
      #endregion
   }
   #endregion

   #region Tests
   [TestMethod]
   public void TrySetNoNotification_WithDifferentValue_ValueChangedAndNoNotification()
   {
      // Arrange
      List<string?> changing = [];
      List<string?> changed = [];

      TestObservable sut = new();
      sut.PropertyChanging += PropertyChanging;
      sut.PropertyChanged += PropertyChanged;

      [ExcludeFromCodeCoverage(Justification = "Is required for checking if test will fail.")]
      void PropertyChanging(object? sender, PropertyChangingEventArgs e) => changing.Add(e.PropertyName);

      [ExcludeFromCodeCoverage(Justification = "Is required for checking if test will fail.")]
      void PropertyChanged(object? sender, PropertyChangedEventArgs e) => changed.Add(e.PropertyName);

      int field = 0;
      const int expectedValue = 1;

      // Act
      bool result = sut.TrySetNoNotification(ref field, expectedValue);

      // Assert
      Assert.That
         .IsTrue(result)
         .AreEqual(field, expectedValue)
         .AreEqual(changing.Count, 0)
         .AreEqual(changed.Count, 0);
   }

   [TestMethod]
   public void TrySetNoNotification_WithSameValue_ValueNotChangedAndNoNotification()
   {
      // Arrange
      List<string?> changing = [];
      List<string?> changed = [];

      TestObservable sut = new();
      sut.PropertyChanging += PropertyChanging;
      sut.PropertyChanged += PropertyChanged;

      [ExcludeFromCodeCoverage(Justification = "Is required for checking if test will fail.")]
      void PropertyChanging(object? sender, PropertyChangingEventArgs e) => changing.Add(e.PropertyName);

      [ExcludeFromCodeCoverage(Justification = "Is required for checking if test will fail.")]
      void PropertyChanged(object? sender, PropertyChangedEventArgs e) => changed.Add(e.PropertyName);

      const int expectedValue = 1;
      int field = expectedValue;

      // Act
      bool result = sut.TrySetNoNotification(ref field, expectedValue);

      // Assert
      Assert.That
         .IsFalse(result)
         .AreEqual(field, expectedValue)
         .AreEqual(changing.Count, 0)
         .AreEqual(changed.Count, 0);
   }

   [TestMethod]
   public void TrySet_WithDifferentValue_ValueChangedAndNotification()
   {
      // Arrange
      List<string?> changing = [];
      List<string?> changed = [];

      TestObservable sut = new();
      sut.PropertyChanging += PropertyChanging;
      sut.PropertyChanged += PropertyChanged;

      void PropertyChanging(object? sender, PropertyChangingEventArgs e) => changing.Add(e.PropertyName);
      void PropertyChanged(object? sender, PropertyChangedEventArgs e) => changed.Add(e.PropertyName);

      int field = 0;
      const int expectedValue = 1;
      const string expectedProperty = "property";

      // Act
      bool result = sut.TrySet(ref field, expectedValue, expectedProperty);

      // Assert
      Assert.That
         .IsTrue(result)
         .AreEqual(field, expectedValue)
         .AreEqual(changing.Count, 1)
         .AreEqual(changing[0], expectedProperty)
         .AreEqual(changed.Count, 1)
         .AreEqual(changed[0], expectedProperty);
   }

   [TestMethod]
   public void TrySet_WithSameValue_ValueNotChangedAndNoNotification()
   {
      // Arrange
      List<string?> changing = [];
      List<string?> changed = [];

      TestObservable sut = new();
      sut.PropertyChanging += PropertyChanging;
      sut.PropertyChanged += PropertyChanged;

      [ExcludeFromCodeCoverage(Justification = "Is required for checking if test will fail.")]
      void PropertyChanging(object? sender, PropertyChangingEventArgs e) => changing.Add(e.PropertyName);

      [ExcludeFromCodeCoverage(Justification = "Is required for checking if test will fail.")]
      void PropertyChanged(object? sender, PropertyChangedEventArgs e) => changed.Add(e.PropertyName);

      const int expectedValue = 1;
      int field = expectedValue;

      // Act
      bool result = sut.TrySet(ref field, expectedValue, "property");

      // Assert
      Assert.That
         .IsFalse(result)
         .AreEqual(field, expectedValue)
         .AreEqual(changing.Count, 0)
         .AreEqual(changed.Count, 0);
   }

   [TestMethod]
   public void RaisePropertyChanged_RaisesForAffectedProperties()
   {
      // Arrange
      List<string?> changing = [];
      List<string?> changed = [];

      TestObservable sut = new();
      sut.PropertyChanging += PropertyChanging;
      sut.PropertyChanged += PropertyChanged;

      [ExcludeFromCodeCoverage(Justification = "Is required for checking if test will fail.")]
      void PropertyChanging(object? sender, PropertyChangingEventArgs e) => changing.Add(e.PropertyName);
      void PropertyChanged(object? sender, PropertyChangedEventArgs e) => changed.Add(e.PropertyName);

      // Act
      sut.RaisePropertyChanged(nameof(TestObservable.A));

      // Assert
      Assert.That
         .AreEqual(changing.Count, 0)
         .AreEqual(changed.Count, 2)
         .AreEqual(changed[0], nameof(TestObservable.A))
         .AreEqual(changed[1], nameof(TestObservable.B));
   }

   [TestMethod]
   public void RaisePropertyChanged_WithNoEventAssigned_DoesNotThrowException()
   {
      // Arrange
      TestObservable sut = new();

      // Act
      void Act() => sut.RaisePropertyChanged(null);

      // Assert
      Assert.That.DoesNotThrowAnyException(Act);
   }

   [TestMethod]
   public void RaisePropertyChanging_RaisesForAffectedProperties()
   {
      // Arrange
      List<string?> changing = [];
      List<string?> changed = [];

      TestObservable sut = new();
      sut.PropertyChanging += PropertyChanging;
      sut.PropertyChanged += PropertyChanged;

      void PropertyChanging(object? sender, PropertyChangingEventArgs e) => changing.Add(e.PropertyName);

      [ExcludeFromCodeCoverage(Justification = "Is required for checking if test will fail.")]
      void PropertyChanged(object? sender, PropertyChangedEventArgs e) => changed.Add(e.PropertyName);

      // Act
      sut.RaisePropertyChanging(nameof(TestObservable.A));

      // Assert
      Assert.That
         .AreEqual(changing.Count, 2)
         .AreEqual(changing[0], nameof(TestObservable.A))
         .AreEqual(changing[1], nameof(TestObservable.B))
         .AreEqual(changed.Count, 0);
   }

   [TestMethod]
   public void RaisePropertyChanging_WithNoEventAssigned_DoesNotThrowException()
   {
      // Arrange
      TestObservable sut = new();

      // Act
      void Act() => sut.RaisePropertyChanging(null);

      // Assert
      Assert.That.DoesNotThrowAnyException(Act);
   }
   #endregion
}
