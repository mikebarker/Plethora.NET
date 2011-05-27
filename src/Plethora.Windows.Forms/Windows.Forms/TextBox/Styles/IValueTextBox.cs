using System;

namespace Plethora.Windows.Forms.Styles
{
    /// <summary>
    /// Interface to be implemented by a TextBox which represents a value.
    /// </summary>
    public interface IValueTextBox
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
        IComparable Value { get; }

        #endregion
    }
}
