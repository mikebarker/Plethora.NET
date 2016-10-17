using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Plethora.Context.Action;

namespace Plethora.Context.Wpf
{
    public class ActionsPrecedenceAdapter : DependencyObject, IActionsAdapter
    {
        public ActionsPrecedenceAdapter()
        {
            this.SetValue(PrecedenceListsPropertyKey, new ObservableCollection<IList<string>>());
        }

        #region PrecedenceLists Dependency Property

        private static readonly DependencyPropertyKey PrecedenceListsPropertyKey = DependencyProperty.RegisterReadOnly(
            "PrecedenceLists",
            typeof(ObservableCollection<IList<string>>),
            typeof(ActionsPrecedenceAdapter),
            new PropertyMetadata(null));

        public static readonly DependencyProperty PrecedenceListsProperty =
            PrecedenceListsPropertyKey.DependencyProperty;

        public ObservableCollection<IList<string>> PrecedenceLists
        {
            get { return (ObservableCollection<IList<string>>)GetValue(PrecedenceListsProperty); }
        }

        #endregion

        public IEnumerable<IAction> Convert(IEnumerable<IAction> actions)
        {
            if ((this.PrecedenceLists == null) || (this.PrecedenceLists.Count == 0))
                return actions;

            foreach (var precedenceList in this.PrecedenceLists)
            {
                actions = ApplyPrecedence(actions, precedenceList);
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
                int index = precedenceList.IndexOf(action.ActionName);
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
    }
}
