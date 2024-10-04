namespace OwlDomain.Common.Observable;

/// <summary>
///   Represents an attribute that marks a property as being notified when other properties change.
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
public sealed class NotifiedByAttribute : Attribute
{
   #region Properties
   /// <summary>The names of the other properties that the marked property should be notified by.</summary>
   public IReadOnlyCollection<string> PropertyNames { get; }
   #endregion

   #region Constructors
   /// <summary>Creates a new instance of the <see cref="NotifiedByAttribute"/>.</summary>
   /// <param name="propertyNames">The names of the other properties that the marked property should be notified by.</param>
   public NotifiedByAttribute(params string[] propertyNames) => PropertyNames = propertyNames;

   /// <summary>Creates a new instance of the <see cref="NotifiedByAttribute"/>.</summary>
   /// <param name="propertyName">The name of the other property that the marked property should be notified by.</param>
   public NotifiedByAttribute(string propertyName) => PropertyNames = [propertyName];
   #endregion
}
