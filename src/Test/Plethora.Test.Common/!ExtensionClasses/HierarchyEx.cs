using Plethora.Collections;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.ExtensionClasses
{
    class HierarchyEx : Hierarchy<IStyle>, IStyle
    {
        public HierarchyEx(params IStyle[] styles)
            : base(styles)
        {
        }


        public string FontName
        {
            get { return GetValue(style => style.FontName); }
        }

        public int? FontSize
        {
            get { return GetValue(style => style.FontSize); }
        }

        public FontProperty FontProperty
        {
            get { return GetValue(style => style.FontProperty, FontProperty.None); }
        }
    }
}
