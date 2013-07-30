using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Plethora.Context.Windows.Forms
{
    public class ContextActionMenuStrip : ContextMenuStrip
    {
        private ContextManager contextManager;

        public ContextManager ContextManager
        {
            get { return contextManager; }
            set { contextManager = value; }
        }

        protected override void OnOpening(System.ComponentModel.CancelEventArgs e)
        {
            base.OnOpening(e);

            this.Items.Clear();

            if (this.contextManager == null)
            {
                Debug.Write("ContextManager not set.");
                return;
            }

            var contexts = this.ContextManager.GetContexts();
            var contextActions = this.ContextManager.GetActions(contexts);

            //Group by the IUiAction.Group property if available, otherwise by ""
            //Order by the IUiAction.Rank property is available
            var groupedActions = contextActions
                .GroupBy(UiActionHelper.GetGroup)
                .Select(group => new {GroupName = group.Key, Actions = group.OrderBy(a=>a, ActionSortOrderComparer.Instance) })
                .OrderBy(g => g.Actions.First(), ActionSortOrderComparer.Instance);
            
            
            bool anyActions = false;
            foreach (var group in groupedActions)
            {
                anyActions = true;

                ToolStripItemCollection itemsCollection; // set to the root menu or a sub-menu
                if (string.Empty.Equals(group.GroupName))
                {
                    itemsCollection = this.Items;
                }
                else
                {
                    var item = new ToolStripMenuItem(group.GroupName);
                    this.Items.Add(item);
                    itemsCollection = item.DropDownItems;
                }

                foreach (IAction contextAction in group.Actions)
                {
                    var action = contextAction;

                    ToolStripItem menuItem = itemsCollection.Add(action.ActionName, null, (sender, args) => action.Execute());
                    menuItem.Enabled = action.CanExecute;

                    menuItem.Image = UiActionHelper.GetImage(action);
                    menuItem.ToolTipText = UiActionHelper.GetActionDescription(action);
                }
            }

            if (!anyActions)
                e.Cancel = true;
        }

        protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
        {
            //Do not hold onto references unnecessarily
            this.Items.Clear();

            base.OnClosed(e);
        }


        private class ActionSortOrderComparer : IComparer<IAction>
        {
            public static readonly ActionSortOrderComparer Instance = new ActionSortOrderComparer();

            public int Compare(IAction x, IAction y)
            {
                int result;
                result = Comparer<int>.Default.Compare(UiActionHelper.GetRank(x), UiActionHelper.GetRank(y));
                if (result != 0)
                    return -result;

                result = Comparer<string>.Default.Compare(x.ActionName, y.ActionName);
                if (result != 0)
                    return result;

                return result;
            }
        }
    }
}
