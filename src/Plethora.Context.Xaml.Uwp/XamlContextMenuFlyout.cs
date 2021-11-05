using Plethora.Context.Action;
using Plethora.Linq;
using Plethora.Xaml.Collections;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Plethora.Context
{
    public class XamlContextMenuFlyoutPlaceHolder : MenuFlyoutItem
    {
        public XamlContextMenuFlyoutPlaceHolder()
        {
            this.Visibility = Visibility.Collapsed;
        }
    }

    public class XamlContextMenuFlyout : MenuFlyout
    {
        private readonly object tagIdentifier = new object();

        public XamlContextMenuFlyout()
        {
            this.Opening += XamlContextMenuFlyout_Opening;
            this.Closed += XamlContextMenuFlyout_Closed;

            XamlContext.SetIsActivityItem(this, true);
        }

        #region MaxGroupItems Dependency Property

        public static readonly DependencyProperty MaxGroupItemsProperty = DependencyProperty.Register(
            nameof(MaxGroupItems),
            typeof(int),
            typeof(XamlContextMenuFlyout),
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
            typeof(XamlContextMenuFlyout),
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
            typeof(XamlContextMenuFlyout),
            new PropertyMetadata(false));

        public bool DisableGrouping
        {
            get { return (bool)this.GetValue(DisableGroupingProperty); }
            set { this.SetValue(DisableGroupingProperty, value); }
        }

        #endregion

        #region SuppressedContextPatterns Dependency Property

        public static readonly DependencyProperty SuppressedContextPatternsProperty = DependencyProperty.Register(
            nameof(SuppressedContextPatterns),
            typeof(StringObservableCollection),
            typeof(XamlContextMenuFlyout),
            new PropertyMetadata(null));

        public StringObservableCollection SuppressedContextPatterns
        {
            get { return (StringObservableCollection)this.GetValue(SuppressedContextPatternsProperty); }
        }

        #endregion

        #region SuppressedActionPatterns Dependency Property

        private static readonly DependencyProperty SuppressedActionPatternsProperty = DependencyProperty.Register(
            nameof(SuppressedActionPatterns),
            typeof(StringObservableCollection),
            typeof(XamlContextMenuFlyout),
            new PropertyMetadata(null));

        public StringObservableCollection SuppressedActionPatterns
        {
            get { return (StringObservableCollection)this.GetValue(SuppressedActionPatternsProperty); }
        }

        #endregion

        #region SuppressedGroupPatterns Dependency Property

        private static readonly DependencyProperty SuppressedGroupPatternsProperty = DependencyProperty.Register(
            nameof(SuppressedGroupPatterns),
            typeof(StringObservableCollection),
            typeof(XamlContextMenuFlyout),
            new PropertyMetadata(null));

        public StringObservableCollection SuppressedGroupPatterns
        {
            get { return (StringObservableCollection)this.GetValue(SuppressedGroupPatternsProperty); }
        }

        #endregion

        #region ActionsAdapter Dependency Property

        public static readonly DependencyProperty ActionsAdapterProperty = DependencyProperty.Register(
            nameof(ActionsAdapter),
            typeof(IActionsAdapter),
            typeof(XamlContextMenuFlyout),
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
            typeof(XamlContextMenuFlyout),
            new PropertyMetadata(null));

        public IValueConverter ImageKeyConverter
        {
            get { return (IValueConverter)this.GetValue(ImageKeyConverterProperty); }
            set { this.SetValue(ImageKeyConverterProperty, value); }
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

        private void ClearItems()
        {
            for (int i = this.Items.Count - 1; i >= 0; i--)
            {
                var item = this.Items[i];
                if (item == null)
                    continue;

                if (ReferenceEquals(item.Tag, tagIdentifier))
                    this.Items.RemoveAt(i);
            }
        }

        private void XamlContextMenuFlyout_Opening(object sender, object e)
        {
            FrameworkElement target = this.Target;

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
                IList<MenuFlyoutItemBase> itemCollection; // set to the root menu or a sub-menu
                if (disableGrouping || group.IsGroupingSuppressed)
                {
                    itemCollection = this.Items;
                    isRootItemCollection = true;
                }
                else
                {
                    var menuSubItem = new MenuFlyoutSubItem();
                    menuSubItem.Tag = tagIdentifier;
                    menuSubItem.Text = group.GroupName;

                    var index = this.Items.IndexOfType(typeof(XamlContextMenuFlyoutPlaceHolder));
                    if (index < 0)
                        index = this.Items.Count;

                    this.Items.Insert(index, menuSubItem);

                    itemCollection = menuSubItem.Items;
                    isRootItemCollection = false;
                }

                foreach (IAction contextAction in group.Actions.Take(maxItems))
                {
                    var action = contextAction;

                    bool canExecute = action.CanExecute;
                    if ((!canExecute) && (!showUnavailableActions))
                        continue;

                    MenuFlyoutItem menuItem = new MenuFlyoutItem();
                    menuItem.Tag = tagIdentifier;
                    menuItem.Text = ActionHelper.GetActionText(action);
                    menuItem.IsEnabled = canExecute;
                    // TODO: menuItem.ToolTip = ActionHelper.GetActionDescription(action);
                    ToolTipService.SetToolTip(menuItem, ActionHelper.GetActionDescription(action));
                    menuItem.Click += delegate { action.Execute(); };

                    if (imageKeyConverter != null)
                    {
                        object imageKey = ActionHelper.GetImageKey(action);

                        if (imageKey != null)
                        {
                            /* TODO:
                            ImageSource imageSource = (ImageSource)imageKeyConverter.Convert(
                                imageKey,
                                typeof(ImageSource),
                                null,
                                CultureInfo.CurrentCulture.Name);

                            menuItem.Icon = new Image { Source = imageSource };
                            */
                        }
                    }

                    if (isRootItemCollection)
                    {
                        var index = itemCollection.IndexOfType(typeof(XamlContextMenuFlyoutPlaceHolder));
                        if (index < 0)
                            index = itemCollection.Count;

                        itemCollection.Insert(index, menuItem);
                    }
                    else
                    {
                        itemCollection.Add(menuItem);
                    }
                }
            }
        }

        private void XamlContextMenuFlyout_Closed(object sender, object e)
        {
            //Do not hold onto references unnecessarily
            this.ClearItems();
        }
    }
}
