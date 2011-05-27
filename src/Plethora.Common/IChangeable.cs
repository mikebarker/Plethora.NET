using System;

namespace Plethora
{
    /// <summary>
    /// Interface defining an object with changeable elements, which result in
    /// notification.
    /// </summary>
    public interface IChangeable
    {
        #region Events

        /// <summary>
        /// Raised when the properties of the <see cref="IChangeable"/> are changed.
        /// </summary>
        event EventHandler Changed;
        #endregion
    }
}
