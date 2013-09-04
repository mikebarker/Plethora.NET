using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Plethora.Context.Windows.Forms
{
    public class ContextActionMenuStrip : ContextMenuStrip
    {
        #region ContextManager Property

        private static readonly object ContextManagerChanged_EventKey = new object();
        private const ContextManager ContextManager_DefaultValue = null;

        /// <summary>
        /// Raised when the value of <see cref="ContextManager"/> has changed.
        /// </summary>
        [Browsable(true)]
        [Category("Action")]
        [Description("Raised when the value of ContextManager has changed")]
        public event EventHandler ContextManagerChanged
        {
            add { base.Events.AddHandler(ContextManagerChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(ContextManagerChanged_EventKey, value); }
        }

        private ContextManager contextManager = ContextManager_DefaultValue;

        /// <summary>
        /// Gets and sets description
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        [DefaultValue(ContextManager_DefaultValue)]
        [Description("Description")]
        public virtual ContextManager ContextManager
        {
            get { return contextManager; }
            set
            {
                if (this.contextManager == value)
                    return;

                this.contextManager = value;
                this.OnContextManagerChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="ContextManagerChanged"/> event.
        /// </summary>
        protected virtual void OnContextManagerChanged(EventArgs e)
        {
            var handler = base.Events[ContextManagerChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region MaxGroupItems Property

        private static readonly object MaxGroupItemsChanged_EventKey = new object();
        private const int MaxGroupItems_DefaultValue = -1;

        /// <summary>
        /// Raised when the value of <see cref="MaxGroupItems"/> has changed.
        /// </summary>
        [Browsable(true)]
        [Category("Action")]
        [Description("Raised when the value of MaxGroupItems has changed")]
        public event EventHandler MaxGroupItemsChanged
        {
            add { base.Events.AddHandler(MaxGroupItemsChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(MaxGroupItemsChanged_EventKey, value); }
        }

        private int maxGroupItems = MaxGroupItems_DefaultValue;

        /// <summary>
        /// Gets and sets the maximum number of items to be displayed in each group.
        /// </summary>
        /// <remarks>
        /// A value less than 0 signifies that the number of displayed items is not limited.
        /// </remarks>
        [Browsable(true)]
        [Category("Behaviour")]
        [DefaultValue(MaxGroupItems_DefaultValue)]
        [Description("The maximum number of items to be displayed in each group.")]
        public virtual int MaxGroupItems
        {
            get { return maxGroupItems; }
            set
            {
                if (this.maxGroupItems == value)
                    return;

                this.maxGroupItems = value;
                this.OnMaxGroupItemsChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="MaxGroupItemsChanged"/> event.
        /// </summary>
        protected virtual void OnMaxGroupItemsChanged(EventArgs e)
        {
            var handler = base.Events[MaxGroupItemsChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region ShowUnavailableActions Property

        private static readonly object ShowUnavailableActionsChanged_EventKey = new object();
        private const bool ShowUnavailableActions_DefaultValue = true;

        /// <summary>
        /// Raised when the value of <see cref="ShowUnavailableActions"/> has changed.
        /// </summary>
        [Browsable(true)]
        [Category("Action")]
        [Description("Raised when the value of ShowUnavailableActions has changed")]
        public event EventHandler ShowUnavailableActionsChanged
        {
            add { base.Events.AddHandler(ShowUnavailableActionsChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(ShowUnavailableActionsChanged_EventKey, value); }
        }

        private bool showUnavailableActions = ShowUnavailableActions_DefaultValue;

        /// <summary>
        /// Gets and sets a flag which determines whether unavailable items should be shown in the menu.
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        [DefaultValue(ShowUnavailableActions_DefaultValue)]
        [Description("A flag which determines whether unavailable items should be shown in the menu.")]
        public virtual bool ShowUnavailableActions
        {
            get { return showUnavailableActions; }
            set
            {
                if (this.showUnavailableActions == value)
                    return;

                this.showUnavailableActions = value;
                this.OnShowUnavailableActionsChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="ShowUnavailableActionsChanged"/> event.
        /// </summary>
        protected virtual void OnShowUnavailableActionsChanged(EventArgs e)
        {
            var handler = base.Events[ShowUnavailableActionsChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region DisableGrouping Property

        private static readonly object DisableGroupingChanged_EventKey = new object();
        private const bool DisableGrouping_DefaultValue = false;

        /// <summary>
        /// Raised when the value of <see cref="DisableGrouping"/> has changed.
        /// </summary>
        [Browsable(true)]
        [Category("Action")]
        [Description("Raised when the value of DisableGrouping has changed")]
        public event EventHandler DisableGroupingChanged
        {
            add { base.Events.AddHandler(DisableGroupingChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(DisableGroupingChanged_EventKey, value); }
        }

        private bool disableGrouping = DisableGrouping_DefaultValue;

        /// <summary>
        /// Gets and sets a flag which determines whether grouping items should be disabled, to all actions in a flat menu.
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        [DefaultValue(DisableGrouping_DefaultValue)]
        [Description("A flag which determines whether unavailable items should be shown in the menu.")]
        public virtual bool DisableGrouping
        {
            get { return disableGrouping; }
            set
            {
                if (this.disableGrouping == value)
                    return;

                this.disableGrouping = value;
                this.OnDisableGroupingChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="DisableGroupingChanged"/> event.
        /// </summary>
        protected virtual void OnDisableGroupingChanged(EventArgs e)
        {
            var handler = base.Events[DisableGroupingChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        protected override void OnOpening(CancelEventArgs e)
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

            //Group by the IUiAction.Group property if available, otherwise by "" as returned from ActionHelper.GetGroup(...)
            //Order by the IUiAction.Rank property is available
            var groupedActions = contextActions
                .GroupBy(ActionHelper.GetGroupSafe)
                .Select(group => new { GroupName = group.Key, Actions = group.OrderBy(a => a, ActionHelper.SortOrderComparer.Instance) })
                .OrderBy(g => g.Actions.First(), ActionHelper.SortOrderComparer.Instance);

            var maxItems = (this.MaxGroupItems < 0)
                               ? int.MaxValue
                               : this.MaxGroupItems;

            bool disableGrouping = this.DisableGrouping;
            bool anyActions = false;
            foreach (var group in groupedActions)
            {
                anyActions = true;

                ToolStripItemCollection itemsCollection; // set to the root menu or a sub-menu
                if (disableGrouping || string.Empty.Equals(group.GroupName))
                {
                    itemsCollection = this.Items;
                }
                else
                {
                    var item = new ToolStripMenuItem(group.GroupName);
                    this.Items.Add(item);
                    itemsCollection = item.DropDownItems;
                }

                foreach (IAction contextAction in group.Actions.Take(maxItems))
                {
                    var action = contextAction;

                    bool canExecute = action.CanExecute;
                    if ((!canExecute) && (!this.ShowUnavailableActions))
                        continue;

                    ToolStripItem menuItem = itemsCollection.Add(action.ActionName, null, (sender, args) => action.Execute());
                    menuItem.Enabled = canExecute;

                    menuItem.Image = ActionHelper.GetImage(action);
                    menuItem.ToolTipText = ActionHelper.GetActionDescription(action);
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

    }
}
