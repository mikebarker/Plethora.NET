using System;
using System.Collections.Generic;

namespace Plethora.Workflow.DAL
{
    public interface IWorkAccessor
    {
        //void InitiateWorkflow(Guid originId, string workflowState, IDictionary<string, string> data);
        void CreateWorkItems(Guid systemId, IEnumerable<WorkItemLite> workItems);

        IEnumerable<WorkItemLite> TryGetWork(Guid systemId, IEnumerable<string> businessStates, int maxWorkItems);

        bool TryGetWorkItemLock(Guid systemId, long workItemId, out DateTime lockTimeoutUtc);
        bool TryRefreshWorkItemLock(Guid systemId, long workItemId, out DateTime lockTimeoutUtc);

        void CompleteWorkItem(Guid systemId, long workItemId, string finalWorkflowState, int startSequenceAt);
        void FailWorkItem(Guid systemId, long workItemId);
    }
}
