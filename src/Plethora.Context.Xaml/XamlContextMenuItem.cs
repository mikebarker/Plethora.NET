using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using Plethora.Collections;
using Plethora.Context.Action;

namespace Plethora.Context
{
    /// <summary>
    /// A WPF <see cref="ContextMenu"/> which auto-populates from the actions currently under context.
    /// </summary>
    public class XamlContextMenuItem : MenuItem
    {
        public XamlContextMenuItem()
        {
            this.SetValue(SuppressedContextPatternsPropertyKey, new StringObservableCollection());
            this.SetValue(SuppressedActionPatternsPropertyKey, new StringObservableCollection());
            this.SetValue(SuppressedGroupPatternsPropertyKey, new StringObservableCollection());

            //Do not show this item in the menu to the user
            // Sub items will be added as appropriate
            this.Visibility = Visibility.Collapsed;
        }

        #region MaxGroupItems Dependency Property

        public static readonly DependencyProperty MaxGroupItemsProperty = DependencyProperty.Register(
            nameof(MaxGroupItems),
            typeof (int),
            typeof (XamlContextMenuItem),
            new PropertyMetadata(-1));

        public int MaxGroupItems
        {
            get { return (int)this.GetValue(MaxGroupItemsProperty); }
            set { this.SetValue(MaxGroupItemsProperty, value); }
        }

        #endregion

        #region ShowUnavailableActions Dependency Property

        public static readonly DependencyProperty ShowUnavailableActionsProperty = DependencyProperty.Register(
            nameof(ShowUnavailableActions),
            typeof(bool),
            typeof(XamlContextMenuItem),
            new PropertyMetadata(true));

        public bool ShowUnavailableActions
        {
            get { return (bool)this.GetValue(ShowUnavailableActionsProperty); }
            set { this.SetValue(ShowUnavailableActionsProperty, value); }
        }

        #endregion

        #region DisableGrouping Dependency Property

        public static readonly DependencyProperty DisableGroupingProperty = DependencyProperty.Register(
            nameof(DisableGrouping),
            typeof(bool),
            typeof(XamlContextMenuItem),
            new PropertyMetadata(false));

        public bool DisableGrouping
        {
            get { return (bool)this.GetValue(DisableGroupingProperty); }
            set { this.SetValue(DisableGroupingProperty, value); }
        }

        #endregion

        #region SuppressedContextPatterns Dependency Property

        public static readonly DependencyPropertyKey SuppressedContextPatternsPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(SuppressedContextPatterns),
            typeof(StringObservableCollection),
            typeof(XamlContextMenuItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SuppressedContextPatternsProperty =
            SuppressedContextPatternsPropertyKey.DependencyProperty;

        public StringObservableCollection SuppressedContextPatterns
        {
            get { return (StringObservableCollection)this.GetValue(SuppressedContextPatternsProperty); }
        }

        #endregion

        #region SuppressedActionPatterns Dependency Property

        private static readonly DependencyPropertyKey SuppressedActionPatternsPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(SuppressedActionPatterns),
            typeof(StringObservableCollection),
            typeof(XamlContextMenuItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SuppressedActionPatternsProperty =
            SuppressedActionPatternsPropertyKey.DependencyProperty;

        public StringObservableCollection SuppressedActionPatterns
        {
            get { return (StringObservableCollection)this.GetValue(SuppressedActionPatternsProperty); }
        }

        #endregion

        #region SuppressedGroupPatterns Dependency Property

        private static readonly DependencyPropertyKey SuppressedGroupPatternsPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(SuppressedGroupPatterns),
            typeof(StringObservableCollection),
            typeof(XamlContextMenuItem),
            new PropertyMetadata(null));

        public static readonly DependencyProperty SuppressedGroupPatternsProperty =
            SuppressedGroupPatternsPropertyKey.DependencyProperty;

        public StringObservableCollection SuppressedGroupPatterns
        {
            get { return (StringObservableCollection)this.GetValue(SuppressedGroupPatternsProperty); }
        }

        #endregion

        #region ActionsAdapter Dependency Property

        public static readonly DependencyProperty ActionsAdapterProperty = DependencyProperty.Register(
            nameof(ActionsAdapter),
            typeof(IActionsAdapter),
            typeof(XamlContextMenuItem),
            new PropertyMetadata(null));

        public IActionsAdapter ActionsAdapter
        {
            get { return (IActionsAdapter)this.GetValue(ActionsAdapterProperty); }
            set { this.SetValue(ActionsAdapterProperty, value); }
        }

        #endregion

        #region ImageKeyConverter Dependency Property

        public static readonly DependencyProperty ImageKeyConverterProperty = DependencyProperty.Register(
            nameof(ImageKeyConverter),
            typeof(IValueConverter),
            typeof(XamlContextMenuItem),
            new PropertyMetadata(null));

        public IValueConverter ImageKeyConverter
        {
            get { return (IValueConverter)this.GetValue(ImageKeyConverterProperty); }
            set { this.SetValue(ImageKeyConverterProperty, value); }
        }

        #endregion

        protected override void OnInitialized(EventArgs e)
        {
            base.OnInitialized(e);

            ContextMenu newContextMenu = this.Parent as ContextMenu;
            if (newContextMenu != null)
            {
                newContextMenu.Opened += this.ContextMenu_Opened;
                newContextMenu.Closed += this.ContextMenu_Closed;

                XamlContext.SetIsActivityItem(newContextMenu, true);
            }
        }

        private void ClearItems()
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

            var contextManager = XamlContext.GetContextManagerForElement(target);
            var actionManager = XamlContext.GetActionManagerForElement(target);

            this.ClearItems();

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

            IValueConverter imageKeyConverter = this.ImageKeyConverter;

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

                    MenuItem menuItem = new MenuItem();
                    menuItem.Tag = this;
                    menuItem.Header = ActionHelper.GetActionText(action);
                    menuItem.IsEnabled = canExecute;
                    menuItem.ToolTip = ActionHelper.GetActionDescription(action);
                    menuItem.Click += delegate { action.Execute(); };

                    if (imageKeyConverter != null)
                    {
                        object imageKey = ActionHelper.GetImageKey(action);

                        if (imageKey != null)
                        {
                            ImageSource imageSource = (ImageSource)imageKeyConverter.Convert(
                                imageKey,
                                typeof(ImageSource),
                                null,
                                CultureInfo.CurrentCulture);

                            menuItem.Icon = new Image { Source = imageSource };
                        }
                    }

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
            this.ClearItems();
        }
    }
}
