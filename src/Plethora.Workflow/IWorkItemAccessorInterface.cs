using System.Collections.Generic;

namespace Plethora.Workflow
{
    internal interface IWorkItemAccessorInterface
    {
        bool TryGetLock(WorkItem workItem);

        bool TryRefreshLock(WorkItem workItem);

        void MarkAsComplete(WorkItem workItem, string finalBusinessState, int startSequenceAt);

        void MarkAsFailed(WorkItem workItem);

        void CreateNewWorkItems(WorkItem workItem, IEnumerable<StateDataPair> stateDataPairs);
    }
}
