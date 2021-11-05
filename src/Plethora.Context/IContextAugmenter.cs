using System.Collections.Generic;

using JetBrains.Annotations;

namespace Plethora.Context
{
    /// <summary>
    /// Interface which allows additional context to be derived from a given context.
    /// </summary>
    /// <remarks>
    /// An instance of <see cref="IContextAugmenter"/> registered with a <see cref="ContextManager"/>
    /// allow the manager to derive additional context from a given context.
    /// </remarks>
    public interface IContextAugmenter
    {
        /// <summary>
        /// The name of the base context.
        /// </summary>
        [NotNull]
        string ContextName { get; }

        /// <summary>
        /// The augmentation function which takes the base context and returns the derived contexts.
        /// </summary>
        /// <param name="context">
        /// The <see cref="ContextInfo"/> to be augmented. When called this context will have
        /// the name as specified by <see cref="ContextName"/>
        /// </param>
        /// <returns>
        ///  Additional context derived from the input context.
        ///  <remarks>
        ///   May be null if no additional context is available.
        ///  </remarks>
        /// </returns>
        [CanBeNull, ItemCanBeNull]
        IEnumerable<ContextInfo> Augment([NotNull] ContextInfo context);
    }
}
