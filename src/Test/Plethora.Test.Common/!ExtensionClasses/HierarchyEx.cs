using Plethora.Collections;
using Plethora.Test.UtilityClasses;

namespace Plethora.Test.ExtensionClasses;

class HierarchyEx(params IStyle[] styles) : Hierarchy<IStyle>(styles), IStyle
{
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
