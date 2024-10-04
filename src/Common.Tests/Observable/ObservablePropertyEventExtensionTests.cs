using System.ComponentModel;

namespace Common.Tests.Observable;

[TestClass]
public sealed class ObservablePropertyEventExtensionTests
{
   #region Tests
   [TestMethod]
   public void IsFor_PropertyChanging_WithNullProperty_ReturnsTrue()
   {
      // Assert
      PropertyChangingEventArgs args = new(null);

      // Act
      bool result = ObservablePropertyEventExtensions.IsFor(args, "any");

      // Assert
      Assert.That.IsTrue(result);
   }

   [TestMethod]
   public void IsFor_PropertyChanging_WithSameProperty_ReturnsTrue()
   {
      // Assert
      const string expectedProperty = "property";
      PropertyChangingEventArgs args = new(expectedProperty);

      // Act
      bool result = ObservablePropertyEventExtensions.IsFor(args, expectedProperty);

      // Assert
      Assert.That.IsTrue(result);
   }

   [TestMethod]
   public void IsFor_PropertyChanging_WithDifferentProperty_ReturnsFalse()
   {
      // Assert
      const string property = "property";
      const string otherProperty = "other";
      PropertyChangingEventArgs args = new(property);

      // Act
      bool result = ObservablePropertyEventExtensions.IsFor(args, otherProperty);

      // Assert
      Assert.That.IsFalse(result);
   }

   [TestMethod]
   public void IsFor_PropertyChanged_WithNullProperty_ReturnsTrue()
   {
      // Assert
      PropertyChangedEventArgs args = new(null);

      // Act
      bool result = ObservablePropertyEventExtensions.IsFor(args, "any");

      // Assert
      Assert.That.IsTrue(result);
   }

   [TestMethod]
   public void IsFor_PropertyChanged_WithSameProperty_ReturnsTrue()
   {
      // Assert
      const string expectedProperty = "property";
      PropertyChangedEventArgs args = new(expectedProperty);

      // Act
      bool result = ObservablePropertyEventExtensions.IsFor(args, expectedProperty);

      // Assert
      Assert.That.IsTrue(result);
   }

   [TestMethod]
   public void IsFor_PropertyChanged_WithDifferentProperty_ReturnsFalse()
   {
      // Assert
      const string property = "property";
      const string otherProperty = "other";
      PropertyChangedEventArgs args = new(property);

      // Act
      bool result = ObservablePropertyEventExtensions.IsFor(args, otherProperty);

      // Assert
      Assert.That.IsFalse(result);
   }
   #endregion
}
