﻿using JetBrains.Annotations;

namespace Plethora.Context.Action
{
    /// <summary>
    /// Extends the <see cref="IAction"/> interface to include visual elements.
    /// </summary>
    public interface IUiAction : IAction
    {
        /// <summary>
        /// Gets the text to be displayed to the user.
        /// </summary>
        [NotNull]
        string ActionText { get; }

        /// <summary>
        /// Gets the description of the action.
        /// </summary>
        /// <example>
        /// A user interface may choose to render the description as a tool-tip.
        /// </example>
        [CanBeNull]
        string ActionDescription { get; }

        /// <summary>
        /// Gets a key of the visual image associated with the action.
        /// </summary>
        [CanBeNull]
        object ImageKey { get; }

        /// <summary>
        /// Gets the group of this <see cref="IUiAction"/>.
        /// </summary>
        /// <remarks>
        /// This group allows multiple actions to be visually grouped together.
        /// </remarks>
        /// <example>
        /// A context menu may choose to place actions within a group in a sub-menu.
        /// </example>
        [CanBeNull]
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
