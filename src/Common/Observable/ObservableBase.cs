namespace OwlDomain.Common.Observable;

/// <summary>
///   Represents the base class for raising notifications about property changes.
/// </summary>
public abstract class ObservableBase : DisposableBase, INotifyPropertyChanging, INotifyPropertyChanged
{
   #region Fields
   private readonly ObservableNotificationLookup _lookup;
   #endregion

   #region Events
   /// <inheritdoc/>
   public event PropertyChangingEventHandler? PropertyChanging;

   /// <inheritdoc/>
   public event PropertyChangedEventHandler? PropertyChanged;
   #endregion

   #region Constructors
   /// <summary>Populates the <see cref="ObservableBase"/> instance.</summary>
   protected ObservableBase()
   {
      Type type = GetType();
      _lookup = ObservableNotificationLookup.GetForType(type);
   }
   #endregion

   #region Methods
   /// <summary>Raises the <see cref="PropertyChanging"/> event for the given <paramref name="propertyName"/>.</summary>
   /// <param name="propertyName">The property name to raise the <see cref="PropertyChanging"/> event for.</param>
   protected virtual void RaisePropertyChanging([CallerMemberName] string? propertyName = null)
   {
      PropertyChanging?.Invoke(this, new(propertyName));
      OnPropertyChanging(propertyName);

      foreach (string other in _lookup.Enumerate(propertyName))
         RaisePropertyChanging(other);
   }

   /// <summary>Raises the <see cref="PropertyChanged"/> event for the given <paramref name="propertyName"/>.</summary>
   /// <param name="propertyName">The property name to raise the <see cref="PropertyChanged"/> event for.</param>
   protected virtual void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
   {
      PropertyChanged?.Invoke(this, new(propertyName));
      OnPropertyChanged(propertyName);

      foreach (string other in _lookup.Enumerate(propertyName))
         RaisePropertyChanged(other);
   }

   /// <summary>Invoked when a property with the given <paramref name="propertyName"/> is changing.</summary>
   /// <param name="propertyName">The name of the property that is changing.</param>
   protected virtual void OnPropertyChanging(string? propertyName = null) { }

   /// <summary>Invoked when a property with the given <paramref name="propertyName"/> has changed.</summary>
   /// <param name="propertyName">The name of the property that has changed.</param>
   protected virtual void OnPropertyChanged(string? propertyName = null) { }

   /// <summary>Tries to set the given <paramref name="value"/> in the <paramref name="field"/>.</summary>
   /// <typeparam name="T">The type of the value to set.</typeparam>
   /// <param name="field">The field to store the value in.</param>
   /// <param name="value">The value to try and set in the given <paramref name="field"/>.</param>
   /// <param name="propertyName">The name of the property that is being set.</param>
   /// <returns>
   ///   <see langword="true"/> if the given <paramref name="value"/> could be set
   ///   to the given <paramref name="field"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>This method will not raise any property change events.</remarks>
   protected bool TrySet<T>(ref T field, T value, [CallerMemberName] string? propertyName = null)
   {
      if (EqualityComparer<T>.Default.Equals(field, value))
         return false;

      RaisePropertyChanging(propertyName);
      field = value;
      RaisePropertyChanged(propertyName);

      return true;
   }

   /// <summary>Tries to set the given <paramref name="value"/> in the <paramref name="field"/>.</summary>
   /// <typeparam name="T">The type of the value to set.</typeparam>
   /// <param name="field">The field to store the value in.</param>
   /// <param name="value">The value to try and set in the given <paramref name="field"/>.</param>
   /// <returns>
   ///   <see langword="true"/> if the given <paramref name="value"/> could be set
   ///   to the given <paramref name="field"/>, <see langword="false"/> otherwise.
   /// </returns>
   /// <remarks>This method will not raise any property change events.</remarks>
#if NET6_0_OR_GREATER
   [SuppressMessage("Performance", "CA1822:Mark members as static", Justification = "Easier to use if it's an instance method instead of a static function.")]
#endif
   protected bool TrySetNoNotification<T>(ref T field, T value)
   {
      if (EqualityComparer<T>.Default.Equals(field, value))
         return false;

      field = value;
      return true;
   }
   #endregion
}

/// <summary>
///   Contains various extension methods related to the <see cref="PropertyChangingEventArgs"/> and <see cref="PropertyChangedEventArgs"/>.
/// </summary>
public static class ObservablePropertyEventExtensions
{
   #region Methods
   /// <summary>Checks whether the given property <paramref name="arguments"/> are for a property with the given <paramref name="propertyName"/>.</summary>
   /// <param name="arguments">The property arguments to check.</param>
   /// <param name="propertyName">The name of the property to check for.</param>
   /// <returns>
   ///   <see langword="true"/> if the given property <paramref name="arguments"/> are for a
   ///   property with the given <paramref name="propertyName"/>, <see langword="false"/> otherwise.
   /// </returns>
   public static bool IsFor(this PropertyChangingEventArgs arguments, string propertyName)
   {
      if (arguments.PropertyName is null)
         return true;

      if (arguments.PropertyName == propertyName)
         return true;

      return false;
   }

   /// <summary>Checks whether the given property <paramref name="arguments"/> are for a property with the given <paramref name="propertyName"/>.</summary>
   /// <param name="arguments">The property arguments to check.</param>
   /// <param name="propertyName">The name of the property to check for.</param>
   /// <returns>
   ///   <see langword="true"/> if the given property <paramref name="arguments"/> are for a
   ///   property with the given <paramref name="propertyName"/>, <see langword="false"/> otherwise.
   /// </returns>
   public static bool IsFor(this PropertyChangedEventArgs arguments, string propertyName)
   {
      if (arguments.PropertyName is null)
         return true;

      if (arguments.PropertyName == propertyName)
         return true;

      return false;
   }
   #endregion
}
