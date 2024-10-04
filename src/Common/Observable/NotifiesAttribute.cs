namespace OwlDomain.Common.Observable;

/// <summary>
///   Represents an attribute that marks a property that notifies other properties when it changes.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class NotifiesAttribute : Attribute
{
   #region Properties
   /// <summary>The names of the other properties that the marked property notifies when it's changed.</summary>
   public IReadOnlyCollection<string> PropertyNames { get; }
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="NotifiesAttribute"/>.</summary>
   /// <param name="propertyNames">The names of the other properties that the marked property notifies when it's changed.</param>
   public NotifiesAttribute(params string[] propertyNames) => PropertyNames = propertyNames;

   /// <summary>Creates a new instance of the <see cref="NotifiesAttribute"/>.</summary>
   /// <param name="propertyName">The name of the other property that the marked property notifies when it's changed.</param>
   public NotifiesAttribute(string propertyName) => PropertyNames = [propertyName];
   #endregion
}
