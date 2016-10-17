using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
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
            base.SetValue(SuppressedContextPatternsPropertyKey, new StringObservableCollection());
            base.SetValue(SuppressedActionPatternsPropertyKey, new StringObservableCollection());
            base.SetValue(SuppressedGroupPatternsPropertyKey, new StringObservableCollection());

            //Do not show this item in the menu to the user
            // Sub items will be added as appropriate
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

        #region SuppressedContextPatterns Dependency Property

        public static readonly DependencyPropertyKey SuppressedContextPatternsPropertyKey = DependencyProperty.RegisterReadOnly(
            "SuppressedContextPatterns",
            typeof(StringObservableCollection),
            typeof(WpfContextMenuItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SuppressedContextPatternsProperty =
            SuppressedContextPatternsPropertyKey.DependencyProperty;

        public StringObservableCollection SuppressedContextPatterns
        {
            get { return (StringObservableCollection)GetValue(SuppressedContextPatternsProperty); }
        }

        #endregion

        #region SuppressedActionPatterns Dependency Property

        private static readonly DependencyPropertyKey SuppressedActionPatternsPropertyKey = DependencyProperty.RegisterReadOnly(
            "SuppressedActionPatterns",
            typeof(StringObservableCollection),
            typeof(WpfContextMenuItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SuppressedActionPatternsProperty =
            SuppressedActionPatternsPropertyKey.DependencyProperty;

        public StringObservableCollection SuppressedActionPatterns
        {
            get { return (StringObservableCollection)GetValue(SuppressedActionPatternsProperty); }
        }

        #endregion

        #region SuppressedGroupPatterns Dependency Property

        private static readonly DependencyPropertyKey SuppressedGroupPatternsPropertyKey = DependencyProperty.RegisterReadOnly(
            "SuppressedGroupPatterns",
            typeof(StringObservableCollection),
            typeof(WpfContextMenuItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SuppressedGroupPatternsProperty =
            SuppressedGroupPatternsPropertyKey.DependencyProperty;

        public StringObservableCollection SuppressedGroupPatterns
        {
            get { return (StringObservableCollection)GetValue(SuppressedGroupPatternsProperty); }
        }

        #endregion

        #region ItemsAdapter Dependency Property

        public static readonly DependencyProperty ActionsAdapterProperty = DependencyProperty.Register(
            "ItemsAdapter",
            typeof(IActionsAdapter),
            typeof(WpfContextMenuItem),
            new PropertyMetadata(null));

        public IActionsAdapter ActionsAdapter
        {
            get { return (IActionsAdapter)GetValue(ActionsAdapterProperty); }
            set { SetValue(ActionsAdapterProperty, value); }
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

                WpfContext.SetIsActivityItem(newContextMenu, true);
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

        void ContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            var parent = this.Parent as ContextMenu;
            if (parent == null)
                return;

            UIElement target = parent.PlacementTarget;

            var contextManager = WpfContext.GetContextManagerForElement(target);
            var actionManager = WpfContext.GetActionManagerForElement(target);

            this.ClearItem();

            var contexts = contextManager.GetContexts();
            contexts = contexts.Where(context => !this.IsSuppressed(context));

            var actions = actionManager.GetActions(contexts);
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

            foreach (var group in groupedActions)
            {
                bool isRootItemCollection;
                ItemCollection itemCollection; // set to the root menu or a sub-menu
                if (disableGrouping || group.IsGroupingSuppressed)
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

                    Uri imageUri = ActionHelper.GetImageUri(action);

                    MenuItem menuItem = new MenuItem();
                    menuItem.Tag = this;
                    menuItem.Header = ActionHelper.GetActionText(action);
                    menuItem.IsEnabled = canExecute;
                    if (imageUri != null)
                        menuItem.Icon = new Image { Source = new BitmapImage(imageUri) };
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

            //Do not show an empty context menu
            if (parent.Items.Count == 0)
                parent.IsOpen = false;
        }

        void ContextMenu_Closed(object sender, RoutedEventArgs e)
        {
            //Do not hold onto references unnecessarily
            this.ClearItem();
        }
    }
}
