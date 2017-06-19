namespace Plethora.Mvvm.Model
{
    /// <summary>
    /// Supports commit and rollback operations.
    /// </summary>
    public interface ITrackChanges : IHasChanged
    {
        /// <summary>
        /// Commit the changes in the instance.
        /// </summary>
        void Commit();

        /// <summary>
        /// Rollback the changes in the instance, restoring the previous state before any changes were made.
        /// </summary>
        void Rollback();
    }
}
