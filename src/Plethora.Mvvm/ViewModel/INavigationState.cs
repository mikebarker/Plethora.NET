namespace Plethora.Mvvm.ViewModel
{
    /// <summary>
    /// A light-weight representation of a <see cref="IViewModel"/> which can be used to hold minimal
    /// state such that the view-model can be recreated when navigated to.
    /// </summary>
    /// <remarks>
    /// By holding only the minimum state required for a <see cref="IViewModel"/>, implementing types
    /// may be persisted in memory but relieve memory pressure. 
    /// </remarks>
    /// <seealso cref="IViewModel"/>
    /// <seealso cref="Navigator"/>
    public interface INavigationState
    {
        /// <summary>
        /// Gets a <see cref="IViewModel"/> represented by this <see cref="INavigationState"/>.
        /// </summary>
        /// <returns>
        /// The <see cref="IViewModel"/> represented by this <see cref="INavigationState"/>.
        /// </returns>
        IViewModel GetViewModel();
    }
}
