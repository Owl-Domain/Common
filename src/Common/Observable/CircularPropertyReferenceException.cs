using System.Text;

namespace OwlDomain.Common.Observable;

/// <summary>
///   Represents an exception for a circular reference.
/// </summary>
/// <param name="parentType">The type that contains the properties in the <paramref name="chain"/>.</param>
/// <param name="chain">The chain of the property references involved in the circular loop.</param>
public sealed class CircularPropertyReferenceException(Type parentType, IReadOnlyList<string> chain)
   : Exception(GenerateMessage(parentType, chain))
{
   #region Properties
   /// <summary>The type that contains the properties in the <see cref="Chain"/>.</summary>
   public Type ParentType { get; } = parentType;

   /// <summary>The chain of the property references involved in the circular loop.</summary>
   public IReadOnlyList<string> Chain { get; } = chain;
   #endregion

   #region Functions
   private static string GenerateMessage(Type parentType, IReadOnlyList<object> chain)
   {
      StringBuilder builder = new();

      builder.Append($"Circular property reference detected in the type ({parentType}).");

      if (builder.Length > 0)
         builder.AppendLine();

      foreach (object node in chain)
      {
         builder
            .Append(node)
            .Append(" -> ");
      }

      if (builder.Length > 0)
      {
         builder
            .Append(chain[0])
            .Append('.');
      }

      string message = builder.ToString();
      return message;
   }
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="CircularPropertyReferenceException"/>.
/// </summary>
public static class CircularReferenceExceptionExtensions
{
   #region Throw methods
   /// <inheritdoc cref="CircularPropertyReferenceException(Type, IReadOnlyList{string})"/>
   [DoesNotReturn, StackTraceHidden, MethodImpl(MethodImplOptions.NoInlining)]
   public static void CircularPropertyReference(this IThrowFor @throw, Type parentType, IReadOnlyList<string> chain)
   {
      throw new CircularPropertyReferenceException(parentType, chain);
   }

   /// <inheritdoc cref="CircularPropertyReferenceException(Type, IReadOnlyList{string})"/>
   [DoesNotReturn, StackTraceHidden, MethodImpl(MethodImplOptions.AggressiveInlining)]
   [ExcludeFromCodeCoverage]
   public static T CircularPropertyReference<T>(this IThrowFor @throw, Type parentType, IReadOnlyList<string> chain)
   {
      CircularPropertyReference(@throw, parentType, chain);
      return default;
   }
   #endregion
}
