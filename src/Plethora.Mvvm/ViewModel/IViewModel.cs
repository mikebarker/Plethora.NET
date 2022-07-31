namespace Plethora.Mvvm.ViewModel
{
    /// <summary>
    /// A view-model that supports a view-model-first paradigm.
    /// </summary>
    public interface IViewModel
    {
        /// <summary>
        /// Gets the view from the <see cref="IViewModel"/>.
        /// </summary>
        object View { get; }

        /// <summary>
        /// Gets the minimal state required to recreate this view model.
        /// </summary>
        INavigationState NavigationState { get; }
    }
}
