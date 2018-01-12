using System;
using System.Collections.Generic;
using System.Linq;

using JetBrains.Annotations;

using Plethora.Logging;
using Plethora.Threading;
using Plethora.Workflow.DAL;

namespace Plethora.Workflow
{
    public class WorkEngine : IDisposable
    {
        private static readonly ILogger log = Logger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly object startStopLock = new object();
        private bool isRunning = false;
        private readonly Guid systemId;
        private readonly ITrigger trigger;
        private readonly IWorkAccessor accessor;
        private readonly Dictionary<string, IWorkProcessor> processorsByState;
        private readonly WorkQueue processorWorkQueue;
        private readonly WorkItemFactory factory;

        public WorkEngine(
            byte processorThreadCount,
            [NotNull] ITrigger trigger,
            [NotNull] IWorkAccessor accessor,
            [NotNull] IDictionary<string, IWorkProcessor> processorsByState)
        {
            if (trigger == null)
                throw new ArgumentNullException(nameof(trigger));

            if (accessor == null)
                throw new ArgumentNullException(nameof(accessor));

            if (processorsByState == null)
                throw new ArgumentNullException(nameof(processorsByState));


            this.systemId = Guid.NewGuid();

            this.trigger = trigger;
            this.accessor = accessor;
            this.factory = new WorkItemFactory(accessor, this.systemId);
            this.processorsByState = processorsByState.ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
            this.processorWorkQueue = new WorkQueue(processorThreadCount, false);

            if (this.processorsByState.Count == 0)
                throw new ArgumentException(ResourceProvider.NoProcessors(), nameof(processorsByState));

            this.processorWorkQueue.ThreadAvailable += this.ThreadAvailable;
            this.trigger.Raised += this.TriggerRaised;
        }

        #region Implementation of IDisposable

        private bool disposed = false;

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
            if (this.disposed)
                return;

            // Check to see if Dispose has already been called.
            if(disposing)
            {
                // Free managed resources.
                this.processorWorkQueue.Dispose();
            }

            // Free unmanaged resources.


            // Note disposing has been done.
            this.disposed = true;
        }

        #endregion

        /// <summary>
        /// Starts the <see cref="WorkEngine"/>.
        /// </summary>
        public void Start()
        {
            lock (this.startStopLock)
            {
                if (this.trigger is IStartStopTrigger)
                    ((IStartStopTrigger)this.trigger).Start();

                this.isRunning = true;
            }
        }

        /// <summary>
        /// Stops the <see cref="WorkEngine"/>.
        /// </summary>
        public void Stop()
        {
            lock (this.startStopLock)
            {
                this.isRunning = false;

                if (this.trigger is IStartStopTrigger)
                    ((IStartStopTrigger)this.trigger).Stop();
            }
        }

        /// <summary>
        /// Gets a flag indicating whether the <see cref="WorkEngine"/> is running.
        /// </summary>
        /// <value>
        /// true if the <see cref="WorkEngine"/> is running; otherwise false.
        /// </value>
        public bool IsRunning
        {
            get
            {
                return this.isRunning;
            }
        }


        private void TriggerRaised(object sender, TriggerRaisedEventArgs e)
        {
            log.Verbose("Trigger raised by {0}",
                sender);

            this.GetWork_Guarded();
        }

        private void ThreadAvailable(object sender, EventArgs e)
        {
            log.Verbose("Work thread has become available.");

            this.GetWork_Guarded();
        }

        private void GetWork_Guarded()
        {
            if (this.IsRunning)
            { 
                this.GetWork();
            }
        }

        private void GetWork()
        {
            int requestWorkCount = this.processorWorkQueue.AvailableThreadCount;
            if (requestWorkCount == 0)
            {
                log.Verbose("No threads available to service get work request.");
                return;
            }

            if (log.IsVerboseEnabled)
            {
                log.Verbose("Attemping to get work (max= {1}) for states= [{0}]",
                    string.Join(", ", this.processorsByState.Keys),
                    requestWorkCount);
            }

            IEnumerable<WorkItemLite> workItemLites = this.accessor.TryGetWork(
                this.systemId,
                this.processorsByState.Keys,
                requestWorkCount);

            bool anyWork = false;
            if (workItemLites != null)
            {
                foreach (WorkItemLite workItemLite in workItemLites)
                {
                    anyWork = true;

                    IWorkProcessor processor;
                    if (this.processorsByState.TryGetValue(workItemLite.State, out processor))
                    {
                        
                        WorkItem workItem = this.factory.CreateWorkItem(workItemLite);

                        log.Debug("Queuing WorkItem [ID= {0:D}, State= {1}] for processing by {2}.",
                            workItem.WorkItemId,
                            workItem.State,
                            processor.GetType().Name);

                        this.processorWorkQueue.BeginInvoke(
                            new Action<IWorkProcessor, WorkItem>(ProcessWorkItem),
                            new object[] {processor, workItem});
                    }
                    else
                    {
                        log.Warn("No processor defined for the state defined the WorkItem [ID= {0:D}, State= {1}].",
                            workItemLite.WorkItemId,
                            workItemLite.State);
                    }
                }
            }

            if (!anyWork)
            {
                log.Verbose("No work available.");
            }
        }

        private static void ProcessWorkItem(IWorkProcessor processor, WorkItem workItem)
        {
            log.Debug("WorkItem [ID= {0:D}, State= {1}] being processed by processor type {2}.",
                workItem.WorkItemId,
                workItem.State,
                processor.GetType().Name);

            try
            {
                processor.Process(workItem);

                log.Verbose("Processing of workItem [ID= {0:D}, State= {1}] complete.",
                    workItem.WorkItemId,
                    workItem.State);
            }
            catch (Exception ex)
            {
                log.Error(ex,
                    "Unhandled exception thrown whilst processing WorkItem [ID= {0:D}, State= {1}] on {2}",
                    workItem.WorkItemId,
                    workItem.State,
                    processor.GetType().Name);

                workItem.Fail();
            }
        }
    }


}
