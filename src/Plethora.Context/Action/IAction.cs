namespace Plethora.Context.Action
{
    /// <summary>
    /// An interface which provides action information.
    /// </summary>
    /// <remarks>
    /// Notice that the memebers of <see cref="IAction"/> can map directly to a WPF <see cref="System.Windows.Input.ICommand"/>.
    /// </remarks>
    public interface IAction
    {
        /// <summary>
        /// Gets the name of the action.
        /// </summary>
        string ActionName { get; }

        /// <summary>
        /// Defines the method that determines whether the command can execute in its current state.
        /// </summary>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        bool CanExecute { get; }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        void Execute();
    }
}
