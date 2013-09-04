using System.Drawing;

namespace Plethora.Context
{
    /// <summary>
    /// A template used to create <see cref="IUiAction"/> instances to operate on
    /// a single <see cref="ContextInfo"/> item.
    /// </summary>
    /// <seealso cref="IActionTemplate"/>
    /// <seealso cref="IUiMultiActionTemplate"/>
    public interface IUiActionTemplate : IActionTemplate
    {
        string GetActionDescription(ContextInfo context);

        Image GetImage(ContextInfo context);

        string GetGroup(ContextInfo context);

        int GetRank(ContextInfo context);
    }

    /// <summary>
    /// A template used to create <see cref="IUiAction"/> instances to operate on
    /// multiple <see cref="ContextInfo"/> items.
    /// </summary>
    /// <seealso cref="IMultiActionTemplate"/>
    /// <seealso cref="IUiActionTemplate"/>
    public interface IUiMultiActionTemplate : IMultiActionTemplate
    {
        string GetActionDescription(ContextInfo[] context);

        Image GetImage(ContextInfo[] context);

        string GetGroup(ContextInfo[] context);

        int GetRank(ContextInfo[] context);
    }
}
