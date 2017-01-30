using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using Plethora.Context.Action;

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

        #region ActionManager Property

        private static readonly object ActionManagerChanged_EventKey = new object();
        private const ActionManager ActionManager_DefaultValue = null;

        /// <summary>
        /// Raised when the value of <see cref="ActionManager"/> has changed.
        /// </summary>
        [Browsable(true)]
        [Category("Action")]
        [Description("Raised when the value of ActionManager has changed")]
        public event EventHandler ActionManagerChanged
        {
            add { base.Events.AddHandler(ActionManagerChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(ActionManagerChanged_EventKey, value); }
        }

        private ActionManager actionManager = ActionManager_DefaultValue;

        /// <summary>
        /// Gets and sets description
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        [DefaultValue(ActionManager_DefaultValue)]
        [Description("Description")]
        public virtual ActionManager ActionManager
        {
            get { return actionManager; }
            set
            {
                if (this.actionManager == value)
                    return;

                this.actionManager = value;
                this.OnActionManagerChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="ActionManagerChanged"/> event.
        /// </summary>
        protected virtual void OnActionManagerChanged(EventArgs e)
        {
            var handler = base.Events[ActionManagerChanged_EventKey] as EventHandler;
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

        #region SuppressedContextPatterns Property

        private static readonly object SuppressedContextPatternsChanged_EventKey = new object();
        private const ICollection<string> SuppressedContextPatterns_DefaultValue = null;

        /// <summary>
        /// Raised when the value of <see cref="SuppressedContextPatterns"/> has changed.
        /// </summary>
        [Browsable(true)]
        [Category("Action")]
        [Description("Raised when the value of SuppressedContextPatterns has changed")]
        public event EventHandler SuppressedContextPatternsChanged
        {
            add { base.Events.AddHandler(SuppressedContextPatternsChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(SuppressedContextPatternsChanged_EventKey, value); }
        }

        private ICollection<string> suppressedContextPatterns = SuppressedContextPatterns_DefaultValue;

        /// <summary>
        /// Gets and sets a list of string patterns for which context with names matching the pattern will be suppressed.
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        [DefaultValue(SuppressedContextPatterns_DefaultValue)]
        [Description("A list of string patterns for which context with names matching the pattern will be suppressed.")]
        public virtual ICollection<string> SuppressedContextPatterns
        {
            get { return suppressedContextPatterns; }
            set
            {
                if (this.suppressedContextPatterns == value)
                    return;

                this.suppressedContextPatterns = value;
                this.OnSuppressedContextPatternsChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="SuppressedContextPatternsChanged"/> event.
        /// </summary>
        protected virtual void OnSuppressedContextPatternsChanged(EventArgs e)
        {
            var handler = base.Events[SuppressedContextPatternsChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region SuppressedActionPatterns Property

        private static readonly object SuppressedActionPatternsChanged_EventKey = new object();
        private const ICollection<string> SuppressedActionPatterns_DefaultValue = null;

        /// <summary>
        /// Raised when the value of <see cref="SuppressedActionPatterns"/> has changed.
        /// </summary>
        [Browsable(true)]
        [Category("Action")]
        [Description("Raised when the value of SuppressedActionPatterns has changed")]
        public event EventHandler SuppressedActionPatternsChanged
        {
            add { base.Events.AddHandler(SuppressedActionPatternsChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(SuppressedActionPatternsChanged_EventKey, value); }
        }

        private ICollection<string> suppressedActionPatterns = SuppressedActionPatterns_DefaultValue;

        /// <summary>
        /// Gets and sets a list of string patterns for which actions with names matching the pattern will be suppressed.
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        [DefaultValue(SuppressedActionPatterns_DefaultValue)]
        [Description("A list of string patterns for which actions with names matching the pattern will be suppressed.")]
        public virtual ICollection<string> SuppressedActionPatterns
        {
            get { return suppressedActionPatterns; }
            set
            {
                if (this.suppressedActionPatterns == value)
                    return;

                this.suppressedActionPatterns = value;
                this.OnSuppressedActionPatternsChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="SuppressedActionPatternsChanged"/> event.
        /// </summary>
        protected virtual void OnSuppressedActionPatternsChanged(EventArgs e)
        {
            var handler = base.Events[SuppressedActionPatternsChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region SuppressedGroupPatterns Property

        private static readonly object SuppressedGroupPatternsChanged_EventKey = new object();
        private const ICollection<string> SuppressedGroupPatterns_DefaultValue = null;

        /// <summary>
        /// Raised when the value of <see cref="SuppressedGroupPatterns"/> has changed.
        /// </summary>
        [Browsable(true)]
        [Category("Action")]
        [Description("Raised when the value of SuppressedGroupPatterns has changed")]
        public event EventHandler SuppressedGroupPatternsChanged
        {
            add { base.Events.AddHandler(SuppressedGroupPatternsChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(SuppressedGroupPatternsChanged_EventKey, value); }
        }

        private ICollection<string> suppressedGroupPatterns = SuppressedGroupPatterns_DefaultValue;

        /// <summary>
        /// Gets and sets a list of string patterns for which groups with names matching the pattern will be suppressed.
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        [DefaultValue(SuppressedGroupPatterns_DefaultValue)]
        [Description("A list of string patterns for which groups with names matching the pattern will be suppressed.")]
        public virtual ICollection<string> SuppressedGroupPatterns
        {
            get { return suppressedGroupPatterns; }
            set
            {
                if (this.suppressedGroupPatterns == value)
                    return;

                this.suppressedGroupPatterns = value;
                this.OnSuppressedGroupPatternsChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="SuppressedGroupPatternsChanged"/> event.
        /// </summary>
        protected virtual void OnSuppressedGroupPatternsChanged(EventArgs e)
        {
            var handler = base.Events[SuppressedGroupPatternsChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region ActionsAdapter Property

        private static readonly object ActionsAdapterChanged_EventKey = new object();
        private const IActionsAdapter ActionsAdapter_DefaultValue = null;

        /// <summary>
        /// Raised when the value of <see cref="ActionsAdapter"/> has changed.
        /// </summary>
        [Browsable(true)]
        [Category("Action")]
        [Description("Raised when the value of ActionsAdapter has changed")]
        public event EventHandler ActionsAdapterChanged
        {
            add { base.Events.AddHandler(ActionsAdapterChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(ActionsAdapterChanged_EventKey, value); }
        }

        private IActionsAdapter actionsAdapter = ActionsAdapter_DefaultValue;

        /// <summary>
        /// Gets and sets an adapter for manipulating the set of available actions.
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        [DefaultValue(ActionsAdapter_DefaultValue)]
        [Description("An adapter for manipulating the set of available actions.")]
        public virtual IActionsAdapter ActionsAdapter
        {
            get { return actionsAdapter; }
            set
            {
                if (this.actionsAdapter == value)
                    return;

                this.actionsAdapter = value;
                this.OnActionsAdapterChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="ActionsAdapterChanged"/> event.
        /// </summary>
        protected virtual void OnActionsAdapterChanged(EventArgs e)
        {
            var handler = base.Events[ActionsAdapterChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        #region ImageKeyConverter Property

        private static readonly object ImageKeyConverterChanged_EventKey = new object();
        private const IImageKeyConverter ImageKeyConverter_DefaultValue = null;

        /// <summary>
        /// Raised when the value of <see cref="ImageKeyConverter"/> has changed.
        /// </summary>
        [Browsable(true)]
        [Category("Action")]
        [Description("Raised when the value of ImageKeyConverter has changed")]
        public event EventHandler ImageKeyConverterChanged
        {
            add { base.Events.AddHandler(ImageKeyConverterChanged_EventKey, value); }
            remove { base.Events.RemoveHandler(ImageKeyConverterChanged_EventKey, value); }
        }

        private IImageKeyConverter imageKeyConverter = ImageKeyConverter_DefaultValue;

        /// <summary>
        /// Gets and sets a converter for obtaining images from their keys.
        /// </summary>
        [Browsable(true)]
        [Category("Behaviour")]
        [DefaultValue(ImageKeyConverter_DefaultValue)]
        [Description("A converter for obtaining images from their keys.")]
        public virtual IImageKeyConverter ImageKeyConverter
        {
            get { return imageKeyConverter; }
            set
            {
                if (this.imageKeyConverter == value)
                    return;

                this.imageKeyConverter = value;
                this.OnImageKeyConverterChanged(EventArgs.Empty);
            }
        }

        /// <summary>
        /// Raises the <see cref="ImageKeyConverterChanged"/> event.
        /// </summary>
        protected virtual void OnImageKeyConverterChanged(EventArgs e)
        {
            var handler = base.Events[ImageKeyConverterChanged_EventKey] as EventHandler;
            if (handler != null)
                handler(this, e);
        }

        #endregion

        private bool IsSuppressed(ContextInfo context)
        {
            if ((this.SuppressedContextPatterns == null) || (this.SuppressedContextPatterns.Count == 0))
                return false;

            return this.SuppressedContextPatterns
                .Any(suppressedAction => WildcardSearch.IsMatch(context.Name, suppressedAction));
        }

        private bool IsSuppressed(IAction action)
        {
            if ((this.SuppressedActionPatterns == null) || (this.SuppressedActionPatterns.Count == 0))
                return false;

            return this.SuppressedActionPatterns
                .Any(suppressedAction => WildcardSearch.IsMatch(action.ActionName, suppressedAction));
        }

        private bool IsGroupingSuppressedFor(string groupName)
        {
            if ((this.SuppressedGroupPatterns == null) || (this.SuppressedGroupPatterns.Count == 0))
                return false;

            return this.SuppressedGroupPatterns
                .Any(suppressedGroup => WildcardSearch.IsMatch(groupName, suppressedGroup));
        }

        protected override void OnOpening(CancelEventArgs e)
        {
            if (this.ContextManager == null)
            {
                Debug.Write("ContextManager not set.");
                e.Cancel = true;
                return;
            }

            if (this.ActionManager == null)
            {
                Debug.Write("ContextManager not set.");
                e.Cancel = true;
                return;
            }

            //e.Cancel can be set to true if no items existed in the Items collection before
            //the method was called. Reset the flag here, and re-test after population.
            e.Cancel = false;

            this.Items.Clear();

            var contexts = this.ContextManager.GetContexts();
            contexts = contexts.Where(context => !this.IsSuppressed(context));

            var actions = this.ActionManager.GetActions(contexts);
            actions = actions.Where(action => !this.IsSuppressed(action));

            if (this.ActionsAdapter != null)
                actions = this.ActionsAdapter.Convert(actions);

            //Group by the IUiAction.Group property if available, otherwise by string.Empty [""] as returned from ActionHelper.GetGroup(...)
            //Order by the IUiAction.Rank property if available
            var groupedActions = actions
                .GroupBy(ActionHelper.GetGroupSafe)
                .Select(group => new
                        {
                            GroupName = group.Key,
                            IsGroupingSuppressed = string.Empty.Equals(group.Key) || this.IsGroupingSuppressedFor(group.Key),
                            Actions = group.OrderBy(a => a, ActionHelper.SortOrderComparer.Instance)
                        })
                .OrderBy(g => g.IsGroupingSuppressed ? 0 : 1)
                .ThenBy(g => g.Actions.First(), ActionHelper.SortOrderComparer.Instance);

            var maxItems = this.MaxGroupItems;
            if (maxItems < 0)
                maxItems = int.MaxValue;

            bool showUnavailableActions = this.ShowUnavailableActions;
            bool disableGrouping = this.DisableGrouping;

            IImageKeyConverter imageKeyConverter = this.ImageKeyConverter;

            bool anyActions = false;
            foreach (var group in groupedActions)
            {
                anyActions = true;

                ToolStripItemCollection itemCollection; // set to the root menu or a sub-menu
                if (disableGrouping || string.Empty.Equals(group.GroupName))
                {
                    itemCollection = this.Items;
                }
                else
                {
                    var item = new ToolStripMenuItem();
                    item.Text = group.GroupName;

                    this.Items.Add(item);
                    itemCollection = item.DropDownItems;
                }

                foreach (IAction contextAction in group.Actions.Take(maxItems))
                {
                    var action = contextAction;

                    bool canExecute = action.CanExecute;
                    if ((!canExecute) && (!showUnavailableActions))
                        continue;

                    ToolStripItem menuItem = new ToolStripMenuItem();
                    menuItem.Tag = this;
                    menuItem.Text = action.ActionName;
                    menuItem.Enabled = canExecute;
                    menuItem.ToolTipText = ActionHelper.GetActionDescription(action);
                    menuItem.Click += delegate { action.Execute(); };

                    if (imageKeyConverter != null)
                    {
                        object imageKey = ActionHelper.GetImageKey(action);

                        if (imageKey != null)
                        {
                            Image image = (Image)imageKeyConverter.Convert(
                                imageKey,
                                CultureInfo.CurrentCulture);

                            menuItem.Image = image;
                        }
                    }
                    itemCollection.Add(menuItem);
                }
            }

            if (!anyActions)
                e.Cancel = true;

            base.OnOpening(e);
        }

        protected override void OnClosed(ToolStripDropDownClosedEventArgs e)
        {
            //Do not hold onto references unnecessarily
            this.Items.Clear();

            base.OnClosed(e);
        }
    }
}
