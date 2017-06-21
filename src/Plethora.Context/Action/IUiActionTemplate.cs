using JetBrains.Annotations;

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
        [NotNull]
        string GetActionText([NotNull] ContextInfo context);

        [CanBeNull]
        string GetActionDescription([NotNull] ContextInfo context);

        [CanBeNull]
        object GetImageKey([NotNull] ContextInfo context);

        [CanBeNull]
        string GetGroup([NotNull] ContextInfo context);

        int GetRank([NotNull] ContextInfo context);
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
        [NotNull]
        string GetActionText([NotNull, ItemNotNull] ContextInfo[] context);

        [CanBeNull]
        string GetActionDescription([NotNull, ItemNotNull] ContextInfo[] context);

        [CanBeNull]
        object GetImageKey([NotNull, ItemNotNull] ContextInfo[] context);

        [CanBeNull]
        string GetGroup([NotNull, ItemNotNull] ContextInfo[] context);

        int GetRank([NotNull, ItemNotNull] ContextInfo[] context);
    }
}
