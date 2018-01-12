using System;
using System.Collections.Generic;

namespace Plethora.Workflow.DAL
{
    /// <summary>
    /// Data access layer object which represents a work item at the persistence level.
    /// </summary>
    public struct WorkItemLite
    {
        public readonly long WorkflowId;
        public readonly long WorkItemId;
        public readonly string Sequence;
        public readonly string State;
        public readonly IDictionary<string, object> Data;
        public readonly DateTime? LockAcquiredUtc;
        public readonly DateTime? LockTimeoutUtc;

        /// <summary>
        /// Initiates a new instance of the <see cref="WorkItemLite"/> struct.
        /// </summary>
        /// <param name="workflowId">The ID of the workflow to which this belongs.</param>
        /// <param name="workItemId">The ID of the work item.</param>
        /// <param name="sequence">The sequence identifier of the work item within the work flow.</param>
        /// <param name="state">The work item's business state.</param>
        /// <param name="data">The data held on the work item.</param>
        /// <param name="lockAcquiredUtc">The UTC time the lock was acquired if acquired.</param>
        /// <param name="lockTimeoutUtc">The UTC time the lock will expire if acquired.</param>
        public WorkItemLite(long workflowId, long workItemId, string sequence, string state, IDictionary<string, object> data, DateTime lockAcquiredUtc, DateTime lockTimeoutUtc)
        {
            this.WorkflowId = workflowId;
            this.WorkItemId = workItemId;
            this.Sequence = sequence;
            this.State = state;
            this.Data = data;
            this.LockAcquiredUtc = lockAcquiredUtc;
            this.LockTimeoutUtc = lockTimeoutUtc;
        }

        /// <summary>
        /// Initiates a new instance of the <see cref="WorkItemLite"/> struct.
        /// </summary>
        /// <param name="workflowId">The ID of the workflow to which this belongs.</param>
        /// <param name="sequence">The sequence identifier of the work item within the work flow.</param>
        /// <param name="state">The work item's business state.</param>
        /// <param name="data">The data held on the work item.</param>
        internal WorkItemLite(long workflowId, string sequence, string state, IDictionary<string, object> data)
        {
            this.WorkflowId = workflowId;
            this.WorkItemId = -1;
            this.Sequence = sequence;
            this.State = state;
            this.Data = data;
            this.LockAcquiredUtc = null;
            this.LockTimeoutUtc = null;
        }
    }
}
