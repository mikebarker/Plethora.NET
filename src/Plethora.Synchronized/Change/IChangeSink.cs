﻿namespace Plethora.Synchronized.Change
{

    /// <summary>
    /// Receives changes from a publisher.
    /// </summary>
    public interface IChangeSink
    {
        /// <summary>
        /// Applie the change which has been received.
        /// </summary>
        /// <param name="change">
        /// The <see cref="ChangeDescriptor"/> received
        /// </param>
        void ApplyChange(ChangeDescriptor change);
    }
}
