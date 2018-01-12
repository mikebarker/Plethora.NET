using System;
using System.Threading;

namespace Plethora.Workflow
{
    public class TimerTrigger : IStartStopTrigger, IDisposable
    {
        private readonly object lockObj = new object();

        private bool isRunning;
        private readonly int period;
        private readonly Timer timer;

        public TimerTrigger(int period)
            :this(period, false)
        {
        }

        public TimerTrigger(int period, bool isRunning)
        {
            this.isRunning = isRunning;

            int dueTime = this.isRunning
                ? 0
                : Timeout.Infinite;

            this.period = period;
            this.timer = new Timer(this.RaiseTrigger, null, dueTime, this.period);
        }


        #region Implementation of IDisposable

        private bool disposed = false;

        ~TimerTrigger()
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
                    this.timer.Dispose();
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
        public event TriggerRaisedEventHandler Raised;

        /// <summary>
        /// Raises the <see cref="Raised"/> event.
        /// </summary>
        protected virtual void OnRaised(TriggerRaisedEventArgs e)
        {
            var handler = this.Raised;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region Implementation of IStartStopTrigger

        public void Start()
        {
            lock (this.lockObj)
            {
                if (!this.isRunning)
                {
                    this.timer.Change(0, this.period);
                    this.isRunning = true;
                }
            }
        }

        public void Stop()
        {
            lock (this.lockObj)
            {
                if (this.isRunning)
                {
                    this.isRunning = false;
                    this.timer.Change(Timeout.Infinite, Timeout.Infinite);
                }
            }
        }

        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }

        #endregion

        private void RaiseTrigger(object state)
        {
            if (this.isRunning)
            {
                this.OnRaised(new TriggerRaisedEventArgs());
            }
        }
    }
}
