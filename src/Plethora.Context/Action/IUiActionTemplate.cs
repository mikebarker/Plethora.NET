using System;

namespace Plethora.Context.Action
{
    /// <summary>
    /// A template used to create <see cref="IUiAction"/> instances to operate on
    /// a single <see cref="ContextInfo"/> item.
    /// </summary>
    /// <seealso cref="IActionTemplate"/>
    /// <seealso cref="IUiAction"/>
    /// <seealso cref="IUiMultiActionTemplate"/>
    public interface IUiActionTemplate : IActionTemplate
    {
        string GetActionText(ContextInfo context);

        string GetActionDescription(ContextInfo context);

        Uri GetImageUri(ContextInfo context);

        string GetGroup(ContextInfo context);

        int GetRank(ContextInfo context);
    }

    /// <summary>
    /// A template used to create <see cref="IUiAction"/> instances to operate on
    /// multiple <see cref="ContextInfo"/> items.
    /// </summary>
    /// <example>
    /// This is used when, for example, multiple trades are selected in a grid.
    /// The list of actions which must be presented should differ from that when
    /// a single trade is selected.
    /// </example>>
    /// <seealso cref="IMultiActionTemplate"/>
    /// <seealso cref="IUiAction"/>
    /// <seealso cref="IUiActionTemplate"/>
    public interface IUiMultiActionTemplate : IMultiActionTemplate
    {
        string GetActionText(ContextInfo[] context);

        string GetActionDescription(ContextInfo[] context);

        Uri GetImageUri(ContextInfo[] context);

        string GetGroup(ContextInfo[] context);

        int GetRank(ContextInfo[] context);
    }
}
