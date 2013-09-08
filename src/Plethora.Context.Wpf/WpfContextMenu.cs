using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace Plethora.Context.Wpf
{
    public class WpfContextMenu : ContextMenu
    {
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

        protected override void OnOpened(RoutedEventArgs e)
        {
            UIElement target = this.PlacementTarget;
            var contextManager = WpfContext.GetManagerForElement(target);

            this.Items.Clear();

            var contexts = contextManager.GetContexts();
            var contextActions = contextManager.GetActions(contexts);

            //Group by the IUiAction.Group property if available, otherwise by "" as returned from ActionHelper.GetGroup(...)
            //Order by the IUiAction.Rank property is available
            var groupedActions = contextActions
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
