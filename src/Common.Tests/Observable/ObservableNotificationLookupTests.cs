using System.Linq;

namespace Common.Tests.Observable;

[TestClass]
public sealed class ObservableNotificationLookupTests
{
   #region Nested types
   [ExcludeFromCodeCoverage]
   private sealed class CircularReferenceType : ObservableBase
   {
      #region Properties
      [Notifies(nameof(B))]
      public int A { get; }

      [Notifies(nameof(C))]
      public int B { get; }

      [Notifies(nameof(A))]
      public int C { get; }
      #endregion
   }

   [ExcludeFromCodeCoverage]
   private sealed class InvalidPropertyNameType : ObservableBase
   {
      #region Properties
      [Notifies("C")]
      public int A { get; }
      #endregion
   }

   [ExcludeFromCodeCoverage]
   private sealed class TestType : ObservableBase
   {
      #region Properties
      [Notifies(nameof(B), nameof(C))]
      public int A { get; }

      [NotifiedBy(nameof(A))]
      public int B { get; }

      [NotifiedBy(nameof(A), nameof(B))]
      public int C { get; }
      #endregion
   }
   [ExcludeFromCodeCoverage]
   private sealed class SelfReferentialPropertyType : ObservableBase
   {
      #region Properties
      [Notifies(nameof(A))]
      public int A { get; }
      #endregion
   }
   #endregion

   #region Tests
   [TestMethod]
   public void GetForType_WithCircularReference_ThrowsCircularPropertyReferenceException()
   {
      // Arrange
      Type type = typeof(CircularReferenceType);

      // Act
      void Act() => _ = ObservableNotificationLookup.GetForType(type);

      // Assert
      Assert.That
         .ThrowsExactException(Act, out CircularPropertyReferenceException exception)
         .AreSameInstance(exception.ParentType, type)
         .AreEqual(exception.Chain.Count, 3)
         .AreEqual(exception.Chain[0], nameof(CircularReferenceType.A))
         .AreEqual(exception.Chain[1], nameof(CircularReferenceType.B))
         .AreEqual(exception.Chain[2], nameof(CircularReferenceType.C));
   }

   [TestMethod]
   public void GetForType_WithSelfReferentialType_ThrowsCircularPropertyReferenceException()
   {
      // Arrange
      Type type = typeof(SelfReferentialPropertyType);

      // Act
      void Act() => _ = ObservableNotificationLookup.GetForType(type);

      // Assert
      Assert.That
         .ThrowsExactException(Act, out CircularPropertyReferenceException exception)
         .AreSameInstance(exception.ParentType, type)
         .AreEqual(exception.Chain.Count, 1)
         .AreEqual(exception.Chain[0], nameof(SelfReferentialPropertyType.A));
   }

   [TestMethod]
   public void GetForType_WithInvalidType_ThrowsMissingMemberException()
   {
      // Arrange
      Type type = typeof(InvalidPropertyNameType);

      // Act
      void Act() => _ = ObservableNotificationLookup.GetForType(type);

      // Assert
      Assert.That.ThrowsExactException<MissingMemberException>(Act);
   }

   [TestMethod]
   public void GetForType_WithValidType_NoExceptionIsThrown()
   {
      // Arrange
      Type type = typeof(TestType);

      // Act
      void Act() => _ = ObservableNotificationLookup.GetForType(type);

      // Assert
      Assert.That.DoesNotThrowAnyException(Act);
   }

   [TestMethod]
   public void Enumerate_WithMarkedProperty_ReturnsExpectedValues()
   {
      // Arrange
      Type type = typeof(TestType);
      ObservableNotificationLookup sut = ObservableNotificationLookup.GetForType(type);

      // Act
      string[] propertiesA = sut.Enumerate(nameof(TestType.A)).ToArray();
      string[] propertiesB = sut.Enumerate(nameof(TestType.B)).ToArray();

      // Assert
      Assert.That
         .AreEqual(propertiesA.Length, 2)
         .AreEqual(propertiesA[0], nameof(TestType.B))
         .AreEqual(propertiesA[1], nameof(TestType.C))
         .AreEqual(propertiesB.Length, 1)
         .AreEqual(propertiesB[0], nameof(TestType.C));
   }

   [TestMethod]
   public void Enumerate_WithNullProperty_ReturnsEmptyCollection()
   {
      // Arrange
      Type type = typeof(TestType);
      ObservableNotificationLookup sut = ObservableNotificationLookup.GetForType(type);

      // Act
      string[] properties = sut.Enumerate(null).ToArray();

      // Assert
      Assert.That.AreEqual(properties.Length, 0);
   }
   #endregion
}
