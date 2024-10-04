using System.Reflection;

namespace OwlDomain.Common.Observable;

/// <summary>
///   Represents a lookup table that stores the property notifications for observable objects.
/// </summary>
public sealed class ObservableNotificationLookup
{
   #region Fields
   private static readonly ConditionalWeakTable<Type, ObservableNotificationLookup> TypeLookups = [];
   private readonly Dictionary<string, HashSet<string>> _properties = [];
   #endregion

   #region Functions
   /// <summary>Gets the lookup table for the given <paramref name="type"/>.</summary>
   /// <param name="type">The type to get the lookup table for.</param>
   /// <returns>The lookup table for the given <paramref name="type"/>.</returns>
   public static ObservableNotificationLookup GetForType(Type type)
   {
      return TypeLookups.GetValue(type, CreateLookup);
   }
   private static ObservableNotificationLookup CreateLookup(Type type)
   {
      ObservableNotificationLookup lookup = new();
      lookup.Populate(type);

      return lookup;
   }
   #endregion

   #region Methods
   /// <summary>Enumerates all of the other properties that should be notified when the given <paramref name="propertyName"/> changes.</summary>
   /// <param name="propertyName">The property name to get the other property names for.</param>
   /// <returns>An enumeration of all of the other properties that should be notified when the given <paramref name="propertyName"/> changes.</returns>
   /// <remarks>This method returns an empty enumerable if the given <paramref name="propertyName"/> is <see langword="null"/>.</remarks>
   public IEnumerable<string> Enumerate(string? propertyName)
   {
      if (propertyName is null)
         yield break;

      if (_properties.TryGetValue(propertyName, out HashSet<string>? properties))
      {
         foreach (string property in properties)
            yield return property;
      }
   }
   private void Populate(Type type)
   {
      const BindingFlags flags =
         BindingFlags.Public |
         BindingFlags.NonPublic |
         BindingFlags.Instance |
         BindingFlags.Static;

      HashSet<string> validPropertyNames = [];
      HashSet<string> usedPropertyNames = [];

      foreach (PropertyInfo property in type.GetProperties(flags))
      {
         validPropertyNames.Add(property.Name);

         foreach (NotifiedByAttribute notifiedBy in property.GetCustomAttributes<NotifiedByAttribute>())
         {
            usedPropertyNames.Add(property.Name);

            foreach (string propertyName in notifiedBy.PropertyNames)
            {
               Populate(propertyName, property.Name);
               usedPropertyNames.Add(propertyName);
            }
         }

         foreach (NotifiesAttribute notifiedBy in property.GetCustomAttributes<NotifiesAttribute>())
         {
            usedPropertyNames.Add(property.Name);

            foreach (string propertyName in notifiedBy.PropertyNames)
            {
               Populate(property.Name, propertyName);
               usedPropertyNames.Add(propertyName);
            }
         }
      }

      foreach (string propertyName in usedPropertyNames)
      {
         if (validPropertyNames.Contains(propertyName) is false)
            Throw.For.MissingMember(type.Name, propertyName);
      }

      ValidateCircularReferences(type);
   }
   private void Populate(string mainPropertyName, string otherPropertyName)
   {
      if (_properties.TryGetValue(mainPropertyName, out HashSet<string>? properties) is false)
      {
         properties = [];
         _properties.Add(mainPropertyName, properties);
      }

      properties.Add(otherPropertyName);
   }
   private void ValidateCircularReferences(Type type)
   {
      HashSet<string> resolved = [];
      HashSet<string> unresolved = [];

      foreach (string property in _properties.Keys)
      {
         resolved.Clear();
         unresolved.Clear();

         ValidateCircularReferences(type, property, resolved, unresolved);
      }
   }

   [StackTraceHidden] // Note(Nightowl): Don't pollute the stack trace with a recursive function;
   private void ValidateCircularReferences(Type type, string property, HashSet<string> resolved, HashSet<string> unresolved)
   {
      unresolved.Add(property);

      if (_properties.TryGetValue(property, out HashSet<string>? properties))
      {
         foreach (string edge in properties)
         {
            if (resolved.Contains(edge))
               continue;

            if (unresolved.Contains(edge))
               Throw.For.CircularPropertyReference(type, [.. unresolved]);

            ValidateCircularReferences(type, edge, resolved, unresolved);
         }
      }

      resolved.Add(property);
      unresolved.Remove(property);
   }
   #endregion
}
