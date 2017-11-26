using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

using Plethora.Context.Action;

namespace Plethora.Context
{
    /// <summary>
    /// An <see cref="IActionsAdapter"/> implementation which filters actions according to a list of inclusive and exclusive patterns.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The precedence list is matched using the <see cref="WildcardSearch.IsMatch"/> to the action name, and may therefore
    /// contain wildcard search patterns.
    /// </para>
    /// <para>
    /// The behaviour depends on which of the pattern lists are populated:
    /// <list type="table">
    ///   <listheader>
    ///     <term>Pattern List</term>
    ///     <term>Behaviour</term>
    ///   </listheader>
    ///   <item>
    ///     <term>Excludes only</term>
    ///     <term>Actions are excluded if the name does match a pattern in the exclude list.</term>
    ///   </item>
    ///   <item>
    ///     <term>Includes only</term>
    ///     <term>Actions are excluded if the name does not match a pattern in the include list.</term>
    ///   </item>
    ///   <item>
    ///     <term>Includes and Excludes</term>
    ///     <term>Actions are excluded if the name matches a pattern in the exclude list, unless it also matches a pattern in the include list.</term>
    ///   </item>
    ///   <item>
    ///     <term>None</term>
    ///     <term>Actions are not excluded, the action list is passed through unalerted.</term>
    ///   </item>
    /// </list>
    /// </para>
    /// </remarks>
    public class ActionsFilterAdapter : DependencyObject, IActionsAdapter
    {
        public ActionsFilterAdapter()
        {
            this.SetValue(IncludePatternsPropertyKey, new ObservableCollection<string>());
            this.SetValue(ExcludePatternsPropertyKey, new ObservableCollection<string>());
        }

        #region IncludePatterns Dependency Property

        private static readonly DependencyPropertyKey IncludePatternsPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(IncludePatterns),
            typeof(ObservableCollection<string>),
            typeof(ActionsFilterAdapter),
            new PropertyMetadata(null));

        public static readonly DependencyProperty IncludePatternsProperty = IncludePatternsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the list of included action name patterns.
        /// </summary>
        public ObservableCollection<string> IncludePatterns
        {
            get { return (ObservableCollection<string>)this.GetValue(IncludePatternsProperty); }
        }

        #endregion

        #region ExcludePatterns Dependency Property

        private static readonly DependencyPropertyKey ExcludePatternsPropertyKey = DependencyProperty.RegisterReadOnly(
            nameof(ExcludePatterns),
            typeof(ObservableCollection<string>),
            typeof(ActionsFilterAdapter),
            new PropertyMetadata(null));

        public static readonly DependencyProperty ExcludePatternsProperty = ExcludePatternsPropertyKey.DependencyProperty;

        /// <summary>
        /// Gets the list of excluded action name patterns.
        /// </summary>
        public ObservableCollection<string> ExcludePatterns
        {
            get { return (ObservableCollection<string>)this.GetValue(ExcludePatternsProperty); }
        }

        #endregion

        public IEnumerable<IAction> Convert(IEnumerable<IAction> actions)
        {
            bool includesEmpty = (this.IncludePatterns == null) || (this.IncludePatterns.Count == 0);
            bool excludesEmpty = (this.ExcludePatterns == null) || (this.ExcludePatterns.Count == 0);

            if (includesEmpty && excludesEmpty)
            {
                return actions;
            }
            else if (!includesEmpty && !excludesEmpty)
            {
                return actions
                    .Where(action =>
                    {
                        bool isExcluded = (GetIndexOf(action.ActionName, this.ExcludePatterns) >= 0);
                        if (isExcluded)
                        {
                            bool isIncluded = (GetIndexOf(action.ActionName, this.IncludePatterns) >= 0);
                            if (isIncluded)
                                return true;
                            else
                                return false;
                        }
                        else
                        {
                            return true;
                        }
                    });
            }
            else if (!includesEmpty && excludesEmpty)
            {
                return actions
                    .Where(action =>
                    {
                        bool isIncluded = (GetIndexOf(action.ActionName, this.IncludePatterns) >= 0);
                        if (isIncluded)
                            return true;
                        else
                            return false;
                    });
            }
            else // if (includesEmpty && !excludesEmpty)
            {
                return actions
                    .Where(action =>
                    {
                        bool isExcluded = (GetIndexOf(action.ActionName, this.ExcludePatterns) >= 0);
                        if (isExcluded)
                            return false;
                        else
                            return true;
                    });
            }
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
