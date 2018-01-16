using System;
using System.Collections.Generic;

namespace Plethora.Workflow.DAL
{
    public interface IWorkAccessor
    {
        /// <summary>
        /// Initiate a new workflow.
        /// </summary>
        /// <param name="systemId">The GUID which uniquely identifies the system initiating the workflow.</param>
        /// <param name="externalId">An ID used by external systems to track this workflow. Can be null.</param>
        /// <param name="workflowState">The initial workflow state.</param>
        /// <param name="description">A human readable description of the workflow for audtiing and tracking purposes. Can be null.</param>
        /// <param name="data">The data to be included in the initial workitem of this workflow.</param>
        /// <returns>
        /// The newly created workflow ID.
        /// </returns>
        long InitiateWorkflow(Guid systemId, string externalId, string workflowState, string description, IDictionary<string, string> data);

        /// <summary>
        /// Create new workitems.
        /// </summary>
        /// <param name="systemId">The GUID which uniquely identifies the system creating the workitems.</param>
        /// <param name="workItems">The workitems to be created.</param>
        void CreateWorkItems(Guid systemId, IEnumerable<WorkItemLite> workItems);


        /// <summary>
        /// Lock and return work from the workflow persistent store.
        /// </summary>
        /// <param name="systemId">The GUID which uniquely identifies the system locking the workitems.</param>
        /// <param name="businessStates">The business states for which workitems may be retrieved.</param>
        /// <param name="maxWorkItems">The maximum number of workitems which may be retrieved.</param>
        /// <returns>
        /// The workitems which have been locked by this system from the workflow persistent store.
        /// </returns>
        IEnumerable<WorkItemLite> TryGetWork(Guid systemId, IEnumerable<string> businessStates, int maxWorkItems);


        /// <summary>
        /// Attempt to get the lock on unlocked workitems.
        /// </summary>
        /// <param name="systemId">The GUID which uniquely identifies the system locking the workitems.</param>
        /// <param name="workItemId">The ID of the workitem to be locked.</param>
        /// <param name="lockTimeoutUtc">Output. The timeout UTC time of the lock.</param>
        /// <returns>
        /// true if the lock was obtained; otherwise false.
        /// </returns>
        bool TryGetWorkItemLock(Guid systemId, long workItemId, out DateTime lockTimeoutUtc);

        /// <summary>
        /// Attempt to refresh the lock on unlocked workitems.
        /// </summary>
        /// <param name="systemId">The GUID which uniquely identifies the system locking the workitems.</param>
        /// <param name="workItemId">The ID of the workitem to be locked.</param>
        /// <param name="lockTimeoutUtc">Output. The timeout UTC time of the lock.</param>
        /// <returns>
        /// true if the lock was refreshed; otherwise false.
        /// </returns>
        bool TryRefreshWorkItemLock(Guid systemId, long workItemId, out DateTime lockTimeoutUtc);


        /// <summary>
        /// Mark the workitem as complete in the workflow persistent store, and proceed with the workflow.
        /// </summary>
        /// <param name="systemId">The GUID which uniquely identifies the system completing the workitems.</param>
        /// <param name="workItemId">The ID of the workitem to be locked.</param>
        /// <param name="finalWorkflowState">The final state of the work item.</param>
        /// <param name="startSequenceAt">An integer which can be used to define the starting numeral of sequence identifier. (used when creating additional workitems in the workflow).</param>
        void CompleteWorkItem(Guid systemId, long workItemId, string finalWorkflowState, int startSequenceAt);

        /// <summary>
        /// Mark the workitem as failed in the workflow persistent store.
        /// </summary>
        /// <param name="systemId">The GUID which uniquely identifies the system failing the workitems.</param>
        /// <param name="workItemId">The ID of the workitem to be locked.</param>
        void FailWorkItem(Guid systemId, long workItemId);
    }
}
