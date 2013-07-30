using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Plethora.Context
{
    public static class UiActionHelper
    {
        public const int DefaultRank = 0;

        public static string GetGroup(IAction action)
        {
            var uiAction = (action as IUiAction);
            if (uiAction == null)
                return string.Empty;

            return uiAction.Group ?? string.Empty;
        }

        public static int GetRank(IAction action)
        {
            var uiAction = (action as IUiAction);
            if (uiAction == null)
                return DefaultRank;

            return uiAction.Rank;
        }

        public static Image GetImage(IAction action)
        {
            var uiAction = (action as IUiAction);
            if (uiAction == null)
                return null;

            return uiAction.Image;
        }

        public static string GetActionDescription(IAction action)
        {
            var uiAction = (action as IUiAction);
            if (uiAction == null)
                return null;

            return uiAction.ActionDescription;
        }
    }
}
