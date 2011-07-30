using System;

namespace Plethora
{
    /// <summary>
    /// Interface which provides a value, and notification of when the value has changed.
    /// </summary>
    public interface IValueProvider
    {
        #region Events

        /// <summary>
        /// Event raised when the <see cref="Value"/> property changes.
        /// </summary>
        event EventHandler ValueChanged;

        #endregion

        #region Properties

        /// <summary>
        /// The value represented by the TextBox
        /// </summary>
        object Value { get; }
        #endregion
    }
}
