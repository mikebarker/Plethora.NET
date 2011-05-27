using System.ComponentModel;

namespace Plethora.Windows.Forms
{
    /// <summary>
    /// Assistance class for working with control and component attributes.
    /// </summary>
    internal static class ControlAttributes
    {
        /// <summary>
        /// Defines string constants to be used with the <see cref="CategoryAttribute"/>.
        /// </summary>
        public static class Category
        {
            #region Fields

            public const string Accessibility = "Accessibility";
            public const string Action = "Action";
            public const string Appearance = "Appearance";
            public const string Behavior = "Behavior";
            public const string Data = "Data";
            public const string Design = "Design";
            public const string Focus = "Focus";
            public const string Layout = "Layout";
            public const string Misc = "Misc";
            #endregion
        }
    }
}
