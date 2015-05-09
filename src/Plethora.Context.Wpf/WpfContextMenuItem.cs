using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Plethora.Collections;
using Plethora.Context.Action;

namespace Plethora.Context.Wpf
{
    /// <summary>
    /// A WPF <see cref="ContextMenu"/> which auto-populates from the actions currently under context.
    /// </summary>
    public class WpfContextMenuItem : MenuItem
    {
        public WpfContextMenuItem()
        {
            SetValue(SuppressedContextsPropertyKey, new StringObservableCollection());
            SetValue(SuppressedActionsPropertyKey, new StringObservableCollection());

            this.Visibility = Visibility.Collapsed;
        }

        #region MaxGroupItems Dependency Property

        public static readonly DependencyProperty MaxGroupItemsProperty = DependencyProperty.Register(
            "MaxGroupItems", 
            typeof (int),
            typeof (WpfContextMenuItem),
            new PropertyMetadata(-1));

        public int MaxGroupItems
        {
            get { return (int)GetValue(MaxGroupItemsProperty); }
            set { SetValue(MaxGroupItemsProperty, value); }
        }

        #endregion

        #region ShowUnavailableActions Dependency Property

        public static readonly DependencyProperty ShowUnavailableActionsProperty = DependencyProperty.Register(
            "ShowUnavailableActions",
            typeof(bool),
            typeof(WpfContextMenuItem),
            new PropertyMetadata(true));

        public bool ShowUnavailableActions
        {
            get { return (bool)GetValue(ShowUnavailableActionsProperty); }
            set { SetValue(ShowUnavailableActionsProperty, value); }
        }

        #endregion

        #region DisableGrouping Dependency Property

        public static readonly DependencyProperty DisableGroupingProperty = DependencyProperty.Register(
            "DisableGrouping",
            typeof(bool),
            typeof(WpfContextMenuItem),
            new PropertyMetadata(false));

        public bool DisableGrouping
        {
            get { return (bool)GetValue(DisableGroupingProperty); }
            set { SetValue(DisableGroupingProperty, value); }
        }

        #endregion

        #region SuppressedContexts Dependency Property

        public static readonly DependencyPropertyKey SuppressedContextsPropertyKey = DependencyProperty.RegisterReadOnly(
            "SuppressedContexts",
            typeof(StringObservableCollection),
            typeof(WpfContextMenuItem),
            new PropertyMetadata(null, SuppressedContextsChanged));

        public static readonly DependencyProperty SuppressedContextsProperty =
            SuppressedContextsPropertyKey.DependencyProperty;

        public StringObservableCollection SuppressedContexts
        {
            get { return (StringObservableCollection)GetValue(SuppressedContextsProperty); }
        }

        private static void SuppressedContextsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var menu = (WpfContextMenuItem)dependencyObject;
            menu.suppressedContextsHashSet = null;
        }

        private HashSet<string> suppressedContextsHashSet;

        private HashSet<string> SuppressedContextsHashSet
        {
            get
            {
                if (this.suppressedContextsHashSet == null)
                {
                    var suppressedContexts = this.SuppressedContexts;
                    if ((suppressedContexts != null) && (suppressedContexts.Count != 0))
                        this.suppressedContextsHashSet = new HashSet<string>(suppressedContexts);
                }

                return this.suppressedContextsHashSet;
            }
        }

        #endregion

        #region SuppressedActions Dependency Property

        private static readonly DependencyPropertyKey SuppressedActionsPropertyKey = DependencyProperty.RegisterReadOnly(
            "SuppressedActions",
            typeof(StringObservableCollection),
            typeof(WpfContextMenuItem),
            new PropertyMetadata(null, SuppressedActionsChanged));

        public static readonly DependencyProperty SuppressedActionsProperty =
            SuppressedActionsPropertyKey.DependencyProperty;

        public StringObservableCollection SuppressedActions
        {
            get { return (StringObservableCollection)GetValue(SuppressedActionsProperty); }
        }

        private static void SuppressedActionsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var menu = (WpfContextMenuItem)dependencyObject;
            menu.suppressedActionsHashSet = null;
        }

        private HashSet<string> suppressedActionsHashSet;

        private HashSet<string> SuppressedActionsHashSet
        {
            get
            {
                if (this.suppressedActionsHashSet == null)
                {
                    var suppressedActions = this.SuppressedActions;
                    if ((suppressedActions != null) && (suppressedActions.Count != 0))
                        this.suppressedActionsHashSet = new HashSet<string>(suppressedActions);
                }

                return this.suppressedActionsHashSet;
            }
        }

        #endregion

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            ContextMenu newContextMenu = this.Parent as ContextMenu;
            if (newContextMenu != null)
            {
                newContextMenu.Opened += ContextMenu_Opened;
                newContextMenu.Closed += ContextMenu_Closed;
            }
        }

        private void ClearItem()
        {
            var parent = this.Parent as ContextMenu;
            if (parent == null)
                return;

            for (int i = parent.Items.Count - 1; i >= 0; i--)
            {
                var item = parent.Items[i] as MenuItem;
                if (item == null)
                    continue;

                if (ReferenceEquals(item.Tag, this))
                    parent.Items.RemoveAt(i);
            }
        }

        void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            var parent = this.Parent as ContextMenu;
            if (parent == null)
                return;

            UIElement target = parent.PlacementTarget;

            var contextManager = WpfContext.GetContextManagerForElement(target);
            var actionManager = WpfContext.GetActionManagerForElement(target);

            ClearItem();

            var contexts = contextManager.GetContexts();
            var suppressedContexts = this.SuppressedContextsHashSet;
            if (suppressedContexts != null)
                contexts = contexts.Where(context => !suppressedContexts.Contains(context.Name));

            var actions = actionManager.GetActions(contexts);
            var suppressedActions = this.SuppressedActionsHashSet;
            if (suppressedActions != null)
                actions = actions.Where(action => !suppressedActions.Contains(action.ActionName));

            //Group by the IUiAction.Group property if available, otherwise by string.Empty [""] as returned from ActionHelper.GetGroup(...)
            //Order by the IUiAction.Rank property if available
            var groupedActions = actions
                .GroupBy(ActionHelper.GetGroupSafe)
                .Select(group => new { GroupName = group.Key, Actions = group.OrderBy(a => a, ActionHelper.SortOrderComparer.Instance) })
                .OrderBy(g => g.Actions.First(), ActionHelper.SortOrderComparer.Instance);

            var maxItems = this.MaxGroupItems;
            if (maxItems < 0)
                maxItems = int.MaxValue;

            bool showUnavailableActions = this.ShowUnavailableActions;
            bool disableGrouping = this.DisableGrouping;

            bool anyActions = false;
            foreach (var group in groupedActions)
            {
                anyActions = true;

                bool isRootItemCollection;
                ItemCollection itemCollection; // set to the root menu or a sub-menu
                if (disableGrouping || string.Empty.Equals(group.GroupName))
                {
                    itemCollection = parent.Items;
                    isRootItemCollection = true;
                }
                else
                {
                    var menuItem = new MenuItem();
                    menuItem.Tag = this;
                    menuItem.Header = group.GroupName;

                    var index = parent.Items.IndexOf(this);
                    parent.Items.Insert(index, menuItem);
                    itemCollection = menuItem.Items;
                    isRootItemCollection = false;
                }

                foreach (IAction contextAction in group.Actions.Take(maxItems))
                {
                    var action = contextAction;

                    bool canExecute = action.CanExecute;
                    if ((!canExecute) && (!showUnavailableActions))
                        continue;

                    MenuItem menuItem = new MenuItem();
                    menuItem.Tag = this;
                    menuItem.Header = action.ActionName;
                    menuItem.IsEnabled = canExecute;
                    menuItem.Icon = ActionHelper.GetImage(action);
                    menuItem.ToolTip = ActionHelper.GetActionDescription(action);
                    menuItem.Click += delegate { action.Execute(); };

                    if (isRootItemCollection)
                    {
                        var index = itemCollection.IndexOf(this);
                        itemCollection.Insert(index, menuItem);
                    }
                    else
                    {
                        itemCollection.Add(menuItem);
                    }
                }
            }

            if (!anyActions)
                this.Visibility = Visibility.Hidden;
        }

        void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            //Do not hold onto references unnecessarily
            ClearItem();
        }
    }
}
