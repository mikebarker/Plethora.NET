using System;

namespace Plethora.Windows.Forms
{
    /// <summary>
    /// Interface to be implemented by a class which provides a value.
    /// </summary>
    public interface IComparableValueProvider
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
