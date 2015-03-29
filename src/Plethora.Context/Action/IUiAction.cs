using System.Drawing;

namespace Plethora.Context.Action
{
    /// <summary>
    /// Extends the <see cref="IAction"/> interface to include visual elements.
    /// </summary>
    public interface IUiAction : IAction
    {
        /// <summary>
        /// Gets the description of the action.
        /// </summary>
        /// <example>
        /// A user interface may choose to render the description as a tool-tip.
        /// </example>
        string ActionDescription { get; }

        /// <summary>
        /// Gets the visual image associated with the action.
        /// </summary>
        Image Image { get; }

        /// <summary>
        /// Gets the group of this <see cref="IUiAction"/>.
        /// </summary>
        /// <remarks>
        /// This group allows multiple actions to be visually grouped together.
        /// </remarks>
        /// <example>
        /// A context menu may choose to place actions within a group in a sub-menu.
        /// </example>
        string Group { get; }

        /// <summary>
        /// Gets the rank of the action.
        /// </summary>
        /// <example>
        /// A user interface may choose to order actions by their rank, or to filter the
        /// list of presented actions to only show the top 10 available actions.
        /// </example>
        int Rank { get; }
    }
}
