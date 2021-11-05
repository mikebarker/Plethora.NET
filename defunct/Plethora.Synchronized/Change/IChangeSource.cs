using System;

namespace Plethora.Synchronized.Change
{
    /// <summary>
    /// Publishes changes to listeners.
    /// </summary>
    public interface IChangeSource
    {
        /// <summary>
        /// Occurs when a change has been applied.
        /// </summary>
        event ChangePublishedEventHandler ChangePublished;
    }


    /// <summary>
    /// Represents the method that handles the <see cref="IChangeSource.ChangePublished"/> event.
    /// </summary>
    /// <param name="sender">The object that raised the event.</param>
    /// <param name="e">Information about the event.</param>
    public delegate void ChangePublishedEventHandler(object sender, ChangePublishedEventArgs e);


    /// <summary>
    /// Provides data for the <see cref="IChangeSource.ChangePublished"/> event.
    /// </summary>
    public class ChangePublishedEventArgs : EventArgs
    {
        #region Fields

        private readonly ChangeDescriptor change;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ChangePublishedEventArgs"/> class.
        /// </summary>
        /// <param name="change">The <see cref="ChangeDescriptor"/> which has been applied.</param>
        public ChangePublishedEventArgs(ChangeDescriptor change)
        {
            //Validation
            if (change == null)
                throw new ArgumentNullException(nameof(change));


            this.change = change;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the <see cref="ChangeDescriptor"/> which has been applied.
        /// </summary>
        public ChangeDescriptor Change
        {
            get { return this.change; }
        }

        #endregion
    }

}
