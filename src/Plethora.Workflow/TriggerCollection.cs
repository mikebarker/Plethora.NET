using System;
using System.Collections.Generic;

namespace Plethora.Workflow;

public class TriggerCollection : IStartStopTrigger, IDisposable
{
    private bool isRunning = false;
    private readonly List<ITrigger> triggers;

    public TriggerCollection(IEnumerable<ITrigger> triggers)
        : this(triggers, false)
    {
    }

    public TriggerCollection(IEnumerable<ITrigger> triggers, bool initialRunningState)
    {
        ArgumentNullException.ThrowIfNull(triggers);

        this.triggers = new(triggers);
        foreach (ITrigger trigger in this.triggers)
        {
            if (trigger is IStartStopTrigger startStopTrigger)
            {
                if (startStopTrigger.IsRunning != initialRunningState)
                {
                    if (initialRunningState)
                        startStopTrigger.Start();
                    else
                        startStopTrigger.Stop();
                }
            }

            trigger.Raised += this.InternalTriggerRaised;
        }
        this.isRunning = initialRunningState;
    }

    #region Implementation of IDisposable

    private bool disposed = false;

    ~TriggerCollection()
    {
        this.Dispose(false);
    }

    /// <summary>
    /// Releasing unmanaged resources.
    /// </summary>
    public void Dispose()
    {
        this.Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <summary>
    /// Release managed and unmanaged resources.
    /// </summary>
    /// <param name="disposing">
    /// If true releases managed and unmanaged resources; otherwise release
    /// only unmanaged resources.
    /// </param>
    protected virtual void Dispose(bool disposing)
    {
        // Check to see if Dispose has already been called.
        if(!this.disposed)
        {
            if(disposing)
            {
                // Dispose managed resources.
                foreach (ITrigger trigger in this.triggers)
                {
                    if (trigger is IDisposable t)
                        t.Dispose();
                }
            }

            // Release unmanaged resources.


            // Note disposing has been done.
            this.disposed = true;
        }
    }
    #endregion

    #region Implementation of ITrigger

    /// <summary>
    /// Raised when the trigger is raised.
    /// </summary>
    public event TriggerRaisedEventHandler? Raised;

    #endregion

    #region Implementation of IStartStopTrigger

    public void Start()
    {
        lock(this.triggers)
        {
            foreach (ITrigger trigger in this.triggers)
            {
                if (trigger is IStartStopTrigger t)
                    t.Start();
            }
            this.isRunning = true;
        }
    }

    public void Stop()
    {
        lock (this.triggers)
        {
            foreach (ITrigger trigger in this.triggers)
            {
                if (trigger is IStartStopTrigger t)
                    t.Stop();
            }
            this.isRunning = false;
        }
    }

    public bool IsRunning
    {
        get
        {
            lock (this.triggers)
            {
                return this.isRunning;
            }
        }
    }

    #endregion

    #region Public Methods

    public void AddTrigger(ITrigger trigger)
    {
        lock (this.triggers)
        {
            if (trigger is IStartStopTrigger startStopTrigger)
            {
                if (startStopTrigger.IsRunning != this.isRunning)
                {
                    if (this.isRunning)
                        startStopTrigger.Start();
                    else
                        startStopTrigger.Stop();
                }
            }

            this.triggers.Add(trigger);

            trigger.Raised += this.InternalTriggerRaised;
        }
    }

    public bool RemoveTrigger(ITrigger trigger)
    {
        lock (this.triggers)
        {
            bool result = this.triggers.Remove(trigger);
            trigger.Raised -= this.InternalTriggerRaised;

            return result;
        }
    }

    #endregion

    private void InternalTriggerRaised(object sender, TriggerRaisedEventArgs eventArgs)
    {
        this.Raised?.Invoke(sender, eventArgs);
    }
}
