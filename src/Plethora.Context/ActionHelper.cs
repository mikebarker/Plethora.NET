﻿using System.Collections.Generic;
using System.Drawing;

namespace Plethora.Context
{
    /// <summary>
    /// Helper class for working with <see cref="IAction"/> and <see cref="IUiAction"/> objects.
    /// </summary>
    public static class ActionHelper
    {
        /// <summary>
        /// The default rank for an <see cref="IAction"/> is not otherwise specified.
        /// </summary>
        public const int DefaultRank = 0;

        /// <summary>
        /// Gets the <see cref="IUiAction.Group"/> property value if the <paramref name="action"/> is
        /// an instance of <see cref="IUiAction"/>, otherwise null.
        /// </summary>
        /// <param name="action">The <see cref="IAction"/> for which the group is required.</param>
        public static string GetGroup(IAction action)
        {
            var uiAction = (action as IUiAction);
            if (uiAction == null)
                return null;

            return uiAction.Group;
        }

        /// <summary>
        /// Gets the <see cref="IUiAction.Group"/> property value if the <paramref name="action"/> is
        /// an instance of <see cref="IUiAction"/>, otherwise string.Empty.
        /// </summary>
        /// <param name="action">The <see cref="IAction"/> for which the group is required.</param>
        /// <returns>
        /// The <see cref="IUiAction.Group"/> property value if available, otherwise string.Empty.
        /// null value are coerced to string.Empty.
        /// </returns>
        /// <seealso cref="GetGroup"/>
        public static string GetGroupSafe(IAction action)
        {
            return GetGroup(action) ?? string.Empty;
        }



        /// <summary>
        /// Gets the <see cref="IUiAction.Rank"/> property value if the <paramref name="action"/> is
        /// an instance of <see cref="IUiAction"/>, otherwise <see cref="ActionHelper.DefaultRank"/>.
        /// </summary>
        /// <param name="action">The <see cref="IAction"/> for which the rank is required.</param>
        public static int GetRank(IAction action)
        {
            var uiAction = (action as IUiAction);
            if (uiAction == null)
                return DefaultRank;

            return uiAction.Rank;
        }

        /// <summary>
        /// Gets the <see cref="IUiAction.Image"/> property value if the <paramref name="action"/> is
        /// an instance of <see cref="IUiAction"/>, otherwise null.
        /// </summary>
        /// <param name="action">The <see cref="IAction"/> for which the image is required.</param>
        public static Image GetImage(IAction action)
        {
            var uiAction = (action as IUiAction);
            if (uiAction == null)
                return null;

            return uiAction.Image;
        }

        /// <summary>
        /// Gets the <see cref="IUiAction.ActionDescription"/> property value if the <paramref name="action"/> is
        /// an instance of <see cref="IUiAction"/>, otherwise null.
        /// </summary>
        /// <param name="action">The <see cref="IAction"/> for which the description is required.</param>
        public static string GetActionDescription(IAction action)
        {
            var uiAction = (action as IUiAction);
            if (uiAction == null)
                return null;

            return uiAction.ActionDescription;
        }


        /// <summary>
        /// Implementation of <see cref="IComparer{T}"/> to compare <see cref="IAction"/> instances, to provide
        /// a visual sort order.
        /// </summary>
        public class SortOrderComparer : IComparer<IAction>
        {
            public static readonly SortOrderComparer Instance = new SortOrderComparer();

            public int Compare(IAction x, IAction y)
            {
                int result;
                result = Comparer<int>.Default.Compare(ActionHelper.GetRank(x), ActionHelper.GetRank(y));
                if (result != 0)
                    return -result;  //Reverse the rank such that highest ranks sort first

                result = Comparer<string>.Default.Compare(x.ActionName, y.ActionName);
                if (result != 0)
                    return result;

                return result;
            }
        }
    }
}