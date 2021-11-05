using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using Plethora.Context.Action;

namespace Plethora.Context
{
    /// <summary>
    /// An <see cref="IActionsAdapter"/> implementation which allows one from a number of actions to proceed
    /// through the adapter.
    /// </summary>
    /// <remarks>
    /// The precedence list is matched using the <see cref="WildcardSearch.IsMatch"/> to the action name, and may therefore
    /// contain wildcard search patterns.
    /// </remarks>
    /// <example>
    /// If the following precedence lists is defined:
    ///     { "Edit Item", "View Item" }
    /// 
    /// When the following actions are past into the adapter:
    ///     { "Edit Item", "View Item", "Sign Item" }
    /// 
    /// The precedence list will ensure that only the highest listed action is selected between the
    /// edit and view actions, returning the action list:
    ///     { "Edit Item", "Sign Item" }
    /// 
    /// since the 'edit' was ranked higher than the 'view' action. The 'sign' action was unranked
    /// and so passed through the adapter.
    /// </example>
    public class ActionsPrecedenceAdapter : DependencyObject, IActionsAdapter
    {
        public ActionsPrecedenceAdapter()
        {
            this.SetValue(PrecedenceListsPropertyKey, new ObservableCollection<IList<string>>());
        }

        #region PrecedenceLists Dependency Property

        private static readonly DependencyPropertyKey PrecedenceListsPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(PrecedenceLists),
            typeof(ObservableCollection<IList<string>>),
            typeof(ActionsPrecedenceAdapter),
            new PropertyMetadata(null));

        public static readonly DependencyProperty PrecedenceListsProperty =
            PrecedenceListsPropertyKey.DependencyProperty;

        public ObservableCollection<IList<string>> PrecedenceLists
        {
            get { return (ObservableCollection<IList<string>>)this.GetValue(PrecedenceListsProperty); }
        }

        #endregion

        public IEnumerable<IAction> Convert(IEnumerable<IAction> actions)
        {
            if ((this.PrecedenceLists == null) || (this.PrecedenceLists.Count == 0))
                return actions;

            foreach (var precedenceList in this.PrecedenceLists)
            {
                actions = this.ApplyPrecedence(actions, precedenceList);
            }

            return actions;
        }

        private IEnumerable<IAction> ApplyPrecedence(IEnumerable<IAction> actions, IList<string> precedenceList)
        {
            if ((precedenceList == null) || (precedenceList.Count == 0))
                return actions;

            List<IAction> unrankedActionList = new List<IAction>();
            List<IAction> rankedActionList = new List<IAction>();

            int precedenceRank = int.MaxValue;
            foreach (var action in actions)
            {
                int index = GetIndexOf(action.ActionName, precedenceList);
                if (index < 0)
                {
                    unrankedActionList.Add(action);
                }
                else
                {
                    if (index == precedenceRank)
                    {
                        rankedActionList.Add(action);
                    }
                    else if (index < precedenceRank)
                    {
                        precedenceRank = index;
                        rankedActionList = new List<IAction>();
                        rankedActionList.Add(action);
                    }
                }
            }

            return rankedActionList.Concat(unrankedActionList);
        }

        private static int GetIndexOf(string actionName, IList<string> patternList)
        {
            for (int i = 0; i < patternList.Count; i++)
            {
                string pattern = patternList[i];

                if (WildcardSearch.IsMatch(actionName, pattern))
                    return i;
            }

            return -1;
        }
    }
}
