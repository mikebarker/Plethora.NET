namespace Plethora.Collections
{
    /// <summary>
    /// Policy describing the action to occur when a duplicate
    /// entry is inserted into a collection.
    /// </summary>
    public enum DuplicatesPolicy
    {
        /// <summary>
        /// Duplicates are allowed.
        /// </summary>
        Allow,

        /// <summary>
        /// The insert or update is ignored, and the collection remains unchanged.
        /// </summary>
        Ignore,

        /// <summary>
        /// The element is replaced by the inserted or updated element.
        /// </summary>
        Replace,

        /// <summary>
        /// The insert or update throws an exception.
        /// </summary>
        Error
    }
}
