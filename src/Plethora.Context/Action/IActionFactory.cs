using System.Collections.Generic;

namespace Plethora.Context.Action
{
    /// <summary>
    /// A factory which creates actions given set of contexts.
    /// </summary>
    /// <seealso cref="TemplateActionFactory"/>
    public interface IActionFactory
    {
        /// <summary>
        /// Generates a list of actions given the set of available contexts.
        /// </summary>
        /// <param name="contextsByName">
        /// The set of contexts, grouped by context name.
        /// </param>
        /// <returns>
        /// The list of actions relevant to the given contexts.
        /// </returns>
        IEnumerable<IAction> GetActions(IDictionary<string, ContextInfo[]> contextsByName);
    }
}
