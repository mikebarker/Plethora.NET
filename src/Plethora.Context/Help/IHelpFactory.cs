using System.Collections.Generic;

using JetBrains.Annotations;

namespace Plethora.Context.Help
{
    /// <summary>
    /// A factory which creates actions given set of contexts.
    /// </summary>
    public interface IHelpFactory
    {
        /// <summary>
        /// Generates a list of help documents given the set of available contexts.
        /// </summary>
        /// <param name="contexts">
        /// The list of contexts.
        /// </param>
        /// <returns>
        /// The list of help documents relevant to the given contexts.
        /// </returns>
        [CanBeNull, ItemCanBeNull]
        IEnumerable<IHelpDocument> GetHelpDocuments([CanBeNull, ItemCanBeNull] IEnumerable<ContextInfo> contexts);
    }
}
