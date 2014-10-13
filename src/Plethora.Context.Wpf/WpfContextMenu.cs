using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Plethora.Context.Wpf
{
    public class StringCollection : ObservableCollection<string>
    {
    }

    public class WpfContextMenu : ContextMenu
    {
        public WpfContextMenu()
        {
            SetValue(SuppressedContextsPropertyKey, new StringCollection());
            SetValue(SuppressedActionsPropertyKey, new StringCollection());
        }

        #region MaxGroupItems Dependency Property

        public static readonly DependencyProperty MaxGroupItemsProperty = DependencyProperty.Register(
            "MaxGroupItems", 
            typeof (int),
            typeof (WpfContextMenu),
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
            typeof(WpfContextMenu),
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
            typeof(WpfContextMenu),
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
            typeof(StringCollection),
            typeof(WpfContextMenu),
            new PropertyMetadata(null, SuppressedContextsChanged));

        public static readonly DependencyProperty SuppressedContextsProperty =
            SuppressedContextsPropertyKey.DependencyProperty;

        public StringCollection SuppressedContexts
        {
            get { return (StringCollection)GetValue(SuppressedContextsProperty); }
        }

        private static void SuppressedContextsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var menu = (WpfContextMenu)dependencyObject;
            menu.suppressedContextsHashSet = null;
        }

        private HashSet<string> suppressedContextsHashSet;

        private HashSet<string> SuppressedContextsHashSet
        {
            get
            {
                if (suppressedContextsHashSet == null)
                {
                    var suppressedContexts = this.SuppressedContexts;
                    if ((suppressedContexts != null) && (suppressedContexts.Count != 0))
                        suppressedContextsHashSet = new HashSet<string>(suppressedContexts);
                }

                return suppressedContextsHashSet;
            }
        }

        #endregion

        #region SuppressedActions Dependency Property

        private static readonly DependencyPropertyKey SuppressedActionsPropertyKey = DependencyProperty.RegisterReadOnly(
            "SuppressedActions",
            typeof (StringCollection),
            typeof (WpfContextMenu),
            new PropertyMetadata(null, SuppressedActionsChanged));

        public static readonly DependencyProperty SuppressedActionsProperty =
            SuppressedActionsPropertyKey.DependencyProperty;

        public StringCollection SuppressedActions
        {
            get { return (StringCollection)GetValue(SuppressedActionsProperty); }
        }

        private static void SuppressedActionsChanged(DependencyObject dependencyObject, DependencyPropertyChangedEventArgs e)
        {
            var menu = (WpfContextMenu)dependencyObject;
            menu.suppressedActionsHashSet = null;
        }

        private HashSet<string> suppressedActionsHashSet;

        private HashSet<string> SuppressedActionsHashSet
        {
            get
            {
                if (suppressedActionsHashSet == null)
                {
                    var suppressedActions = this.SuppressedActions;
                    if ((suppressedActions != null) && (suppressedActions.Count != 0))
                        suppressedActionsHashSet = new HashSet<string>(suppressedActions);
                }

                return suppressedActionsHashSet;
            }
        }

        #endregion


        protected override void OnOpened(RoutedEventArgs e)
        {
            UIElement target = this.PlacementTarget;
            var contextManager = WpfContext.GetContextManagerForElement(target);
            var actionManager = WpfContext.GetActionManagerForElement(target);

            this.Items.Clear();

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

                ItemCollection itemCollection; // set to the root menu or a sub-menu
                if (disableGrouping || string.Empty.Equals(group.GroupName))
                {
                    itemCollection = this.Items;
                }
                else
                {
                    var item = new MenuItem();
                    item.Header = group.GroupName;

                    this.Items.Add(item);
                    itemCollection = item.Items;
                }

                foreach (IAction contextAction in group.Actions.Take(maxItems))
                {
                    var action = contextAction;

                    bool canExecute = action.CanExecute;
                    if ((!canExecute) && (!showUnavailableActions))
                        continue;

                    MenuItem menuItem = new MenuItem();
                    menuItem.Header = action.ActionName;
                    menuItem.IsEnabled = canExecute;
                    menuItem.Icon = ActionHelper.GetImage(action);
                    menuItem.ToolTip = ActionHelper.GetActionDescription(action);
                    menuItem.Click += delegate { action.Execute(); };

                    itemCollection.Add(menuItem);
                }
            }

            if (!anyActions)
                this.Visibility= Visibility.Hidden;

            base.OnOpened(e);
        }

        protected override void OnClosed(RoutedEventArgs e)
        {
            //Do not hold onto references unnecessarily
            this.Items.Clear();

            base.OnClosed(e);
        }
    }
}
