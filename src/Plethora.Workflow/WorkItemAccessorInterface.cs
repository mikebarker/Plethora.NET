using System;
using System.Collections.Generic;
using System.Globalization;

using Plethora.Logging;
using Plethora.Workflow.DAL;

namespace Plethora.Workflow
{
    internal sealed class WorkItemAccessorInterface : IWorkItemAccessorInterface
    {
        private static readonly ILogger log = Logger.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Guid systemId;
        private readonly IWorkAccessor workAccessor;

        public WorkItemAccessorInterface(IWorkAccessor workAccessor, Guid systemId)
        {
            // Validation
            if (workAccessor == null)
                throw new ArgumentNullException(nameof(workAccessor));


            this.workAccessor = workAccessor;
            this.systemId = systemId;
        }

        public bool TryGetLock(WorkItem workItem)
        {
            //Validation
            if (workItem == null)
                throw new ArgumentNullException(nameof(workItem));

            if (workItem.IsLockAcquired)
                throw new InvalidOperationException(ResourceProvider.LockAlreadyAcquired());


            DateTime lockAcquiredUtc = DateTime.UtcNow;

            DateTime lockTimeoutUtc;
            try
            {
                if (!this.workAccessor.TryGetWorkItemLock(this.systemId, workItem.WorkItemId, out lockTimeoutUtc))
                    return false;
            }
            catch (Exception ex)
            {
                log.Error(ex, "An unhandled error was thrown whilst trying to get a lock on WorkItem [ID= {0:D}, State= {1}] by the accessor {2}",
                    workItem.WorkItemId,
                    workItem.State,
                    this.workAccessor);
                throw;
            }

            workItem.RefreshLockTimes(
                lockAcquiredUtc,
                lockTimeoutUtc);

            return true;
        }

        public bool TryRefreshLock(WorkItem workItem)
        {
            //Validation
            if (workItem == null)
                throw new ArgumentNullException(nameof(workItem));

            if (!workItem.IsLockAcquired)
                throw new InvalidOperationException(ResourceProvider.LockNotHeld());


            DateTime lockAcquiredUtc = DateTime.UtcNow;

            DateTime lockTimeoutUtc;
            try
            {
                if (!this.workAccessor.TryRefreshWorkItemLock(this.systemId, workItem.WorkItemId, out lockTimeoutUtc))
                    return false;
            }
            catch (Exception ex)
            {
                log.Error(ex, "An unhandled error was thrown whilst trying to refresh the lock on WorkItem [ID= {0:D}, State= {1}] by the accessor {2}",
                    workItem.WorkItemId,
                    workItem.State,
                    this.workAccessor);
                throw;
            }

            workItem.RefreshLockTimes(
                lockAcquiredUtc,
                lockTimeoutUtc);

            return true;
        }

        public void MarkAsComplete(WorkItem workItem, string finalState, int startSequenceAt)
        {
            //Validation
            if (workItem == null)
                throw new ArgumentNullException(nameof(workItem));


            try
            {
                this.workAccessor.CompleteWorkItem(this.systemId, workItem.WorkItemId, finalState, startSequenceAt);
            }
            catch (Exception ex)
            {
                log.Error(ex, "An unhandled error was thrown whilst trying to complete the WorkItem [ID= {0:D}, State= {1}] by the accessor {2}",
                    workItem.WorkItemId,
                    workItem.State,
                    this.workAccessor);
                throw;
            }
        }

        public void MarkAsFailed(WorkItem workItem)
        {
            //Validation
            if (workItem == null)
                throw new ArgumentNullException(nameof(workItem));


            try
            {
                this.workAccessor.FailWorkItem(this.systemId, workItem.WorkItemId);
            }
            catch (Exception ex)
            {
                log.Error(ex, "An unhandled error was thrown whilst trying to fail the WorkItem [ID= {0:D}, State= {1}] by the accessor {2}",
                    workItem.WorkItemId,
                    workItem.State,
                    this.workAccessor);
                throw;
            }
        }

        public void CreateNewWorkItems(WorkItem workItem, IEnumerable<StateDataPair> stateDataPairs)
        {
            //Validation
            if (workItem == null)
                throw new ArgumentNullException(nameof(workItem));


            List<WorkItemLite> list = new List<WorkItemLite>();

            int sequenceCount = 1;
            foreach (StateDataPair stateDataPair in stateDataPairs)
            {
                string sequence = workItem.Sequence + "-" + sequenceCount.ToString(CultureInfo.InvariantCulture);
                sequenceCount += 1;

                WorkItemLite workItemLite = new WorkItemLite(
                    workItem.WorkflowId,
                    sequence,
                    stateDataPair.State,
                    stateDataPair.Data);

                list.Add(workItemLite);
            }

            try
            {
                this.workAccessor.CreateWorkItems(this.systemId, list);
            }
            catch (Exception ex)
            {
                log.Error(ex, "An unhandled error was thrown whilst trying to create work items by the accessor {0}",
                    this.workAccessor);
                throw;
            }
        }
    }
}
