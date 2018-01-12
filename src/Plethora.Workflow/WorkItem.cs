using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Transactions;

namespace Plethora.Workflow
{
    public sealed class WorkItem : IEquatable<WorkItem>
    {
        #region Fields

        private readonly long workflowId;
        private readonly long workItemId;
        private readonly string sequence;
        private readonly string state;
        private readonly Dictionary<string, object> data;
        private DateTime? lockTimeoutUtc = null;
        private DateTime? lockAcquiredUtc = null;

        private readonly IWorkItemAccessorInterface accessorInterface;

        #endregion

        #region Constructors

        internal WorkItem(long workflowId, long workItemId, string sequence, string state, IDictionary<string, object> data,
            DateTime lockAcquiredUtc,
            DateTime lockTimeoutUtc,
            IWorkItemAccessorInterface accessorInterface)
            : this(workflowId, workItemId, sequence, state, data, accessorInterface)
        {
            this.RefreshLockTimes(
                lockAcquiredUtc,
                lockTimeoutUtc);
        }


        internal WorkItem(long workflowId, long workItemId, string sequence, string state, IDictionary<string, object> data,
            IWorkItemAccessorInterface accessorInterface)
        {
            //Validation
            if (sequence == null)
                throw new ArgumentNullException(nameof(sequence));

            if (state == null)
                throw new ArgumentNullException(nameof(state));

            if (data == null)
                throw new ArgumentNullException(nameof(data));

            if (accessorInterface == null)
                throw new ArgumentNullException(nameof(accessorInterface));


            this.workflowId = workflowId;
            this.workItemId = workItemId;
            this.sequence = sequence;
            this.state = state;
            this.data = new Dictionary<string, object>(data);
            this.accessorInterface = accessorInterface;
        }

        #endregion

        #region Properties

        public long WorkflowId
        {
            get { return this.workflowId; }
        }

        public long WorkItemId
        {
            get { return this.workItemId; }
        }

        public string State
        {
            get { return this.state; }
        }

        public string Sequence
        {
            get { return this.sequence; }
        }

        public IDictionary<string, object> Data
        {
            get { return new ReadOnlyDictionary<string, object>(this.data); }
        }

        public bool IsLockAcquired
        {
            get { return (this.lockTimeoutUtc != null); }
        }

        public DateTime LockTimeoutUtc
        {
            get
            {
                if (!this.IsLockAcquired)
                    throw new InvalidOperationException(ResourceProvider.LockNotHeld());

                return this.lockTimeoutUtc.Value;
            }
        }

        public DateTime LockAcquiredTimeUtc
        {
            get
            {
                if (!this.IsLockAcquired)
                    throw new InvalidOperationException(ResourceProvider.LockNotHeld());

                return this.lockAcquiredUtc.Value;
            }
        }

        #endregion

        #region Methods

        public bool TryRefreshLock()
        {
            return this.accessorInterface.TryRefreshLock(this);
        }

        public bool TryGetLock()
        {
            if (this.IsLockAcquired)
                throw new InvalidOperationException(ResourceProvider.LockAlreadyAcquired());

            return this.accessorInterface.TryGetLock(this);
        }

        public void Complete(string newState)
        {
            if (!this.IsLockAcquired)
                throw new InvalidOperationException(ResourceProvider.LockNotHeld());

            this.accessorInterface.MarkAsComplete(this, newState, 1);

            this.ClearLock();
        }

        public void CompleteWithNewWork(
            string newState,
            ICollection<StateDataPair> newWorkItems)
        {
            if (!this.IsLockAcquired)
                throw new InvalidOperationException(ResourceProvider.LockNotHeld());

            using (var txn = new TransactionScope(TransactionScopeOption.Required))
            {
                this.accessorInterface.CreateNewWorkItems(this, newWorkItems);
                this.accessorInterface.MarkAsComplete(this, newState, newWorkItems.Count + 1);

                txn.Complete();
            }

            this.ClearLock();
        }

        public void Fail()
        {
            if (!this.IsLockAcquired)
                throw new InvalidOperationException(ResourceProvider.LockNotHeld());

            this.accessorInterface.MarkAsFailed(this);

            this.ClearLock();
        }

        internal void RefreshLockTimes(
            DateTime lockAcquiredUtc,
            DateTime lockTimeoutUtc)
        {
            //Validate
            if (lockAcquiredUtc > DateTime.UtcNow)
                throw new ArgumentException(ResourceProvider.AcquiredTimeMustNotBeInFuture());

            if (lockAcquiredUtc >= lockTimeoutUtc)
                throw new ArgumentException(ResourceProvider.TimeoutMustBeAfterAcquired());


            this.lockAcquiredUtc = lockAcquiredUtc;
            this.lockTimeoutUtc = lockTimeoutUtc;
        }

        private void ClearLock()
        {
            this.lockAcquiredUtc = null;
            this.lockTimeoutUtc = null;
        }

        #endregion

        #region Equatable

        public bool Equals(WorkItem other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.workItemId == other.workItemId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return this.Equals((WorkItem)obj);
        }

        public override int GetHashCode()
        {
            return this.workItemId.GetHashCode();
        }

        #endregion
    }

    internal static class WorkItemHelper
    {
        public static bool ShouldRefreshLock(this WorkItem workItem)
        {
            return ShouldRefreshLock(workItem, 0.5);
        }

        public static bool ShouldRefreshLock(this WorkItem workItem, double timeoutFactor)
        {
            if (!workItem.IsLockAcquired)
                return false;

            TimeSpan timeout = workItem.LockTimeoutUtc.Subtract(workItem.LockAcquiredTimeUtc);
            double refreshMilliseconds = timeout.TotalMilliseconds * timeoutFactor;


            DateTime refreshTimeUtc = workItem.LockTimeoutUtc.AddMilliseconds(-refreshMilliseconds);
            return (refreshTimeUtc <= DateTime.UtcNow);
        }
    }
}
