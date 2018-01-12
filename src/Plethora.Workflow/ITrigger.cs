using System;

namespace Plethora.Workflow
{
    public delegate void TriggerRaisedEventHandler(object sender, TriggerRaisedEventArgs eventArgs);

    public class TriggerRaisedEventArgs : EventArgs
    {
    }

    public interface ITrigger
    {
        /// <summary>
        /// Raised when the trigger is raised.
        /// </summary>
        event TriggerRaisedEventHandler Raised;
    }

    public interface IStartStopTrigger : ITrigger
    {
        /// <summary>
        /// Starts the trigger.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the trigger.
        /// </summary>
        void Stop();

        /// <summary>
        /// Gets a flag indicating whether the trigger is running.
        /// </summary>
        /// <value>
        /// true if the trigger is running; otherwise false.
        /// </value>
        bool IsRunning { get; }
    }
}
