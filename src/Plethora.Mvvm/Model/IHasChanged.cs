namespace Plethora.Mvvm.Model
{
    /// <summary>
    /// An interface which exposes a property to determine whether the implementing instance has changed values.
    /// </summary>
    public interface IHasChanged
    {
        /// <summary>
        /// Gets a flag indicating whether the instance has changed values.
        /// </summary>
        /// <returns>
        /// true if the implementing instance has changed; otherwise false.
        /// </returns>
        bool HasChanged { get; }
    }
}
