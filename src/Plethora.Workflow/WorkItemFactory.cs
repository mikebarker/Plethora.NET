using System;

using Plethora.Workflow.DAL;

namespace Plethora.Workflow
{
    internal sealed class WorkItemFactory
    {
        private readonly WorkItemAccessorInterface accessorInterface;

        public WorkItemFactory(IWorkAccessor workAccessor, Guid systemId)
        {
            // Validation
            if (workAccessor == null)
                throw new ArgumentNullException(nameof(workAccessor));


            this.accessorInterface = new WorkItemAccessorInterface(workAccessor, systemId);
        }

        public WorkItem CreateWorkItem(WorkItemLite workItemLite)
        {
            WorkItem workItem = (workItemLite.LockTimeoutUtc != null)
                ? this.CreateWorkItemLocked(workItemLite)
                : this.CreateWorkItemUnlocked(workItemLite);

            return workItem;
        }

        private WorkItem CreateWorkItemLocked(WorkItemLite workItemLite)
        {
            WorkItem workItem = new WorkItem(
                workItemLite.WorkflowId,
                workItemLite.WorkItemId,
                workItemLite.Sequence,
                workItemLite.State,
                workItemLite.Data,
                workItemLite.LockAcquiredUtc.Value,
                workItemLite.LockTimeoutUtc.Value,

                this.accessorInterface);

            return workItem;
        }

        private WorkItem CreateWorkItemUnlocked(WorkItemLite workItemLite)
        {
            WorkItem workItem = new WorkItem(
                workItemLite.WorkflowId,
                workItemLite.WorkItemId,
                workItemLite.Sequence,
                workItemLite.State,
                workItemLite.Data,

                this.accessorInterface);

            return workItem;
        }
    }
}
