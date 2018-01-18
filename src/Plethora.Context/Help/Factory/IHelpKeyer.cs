using JetBrains.Annotations;

namespace Plethora.Context.Help.Factory
{
    /// <summary>
    /// Interface defining the methods required to get the help key.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    public interface IHelpKeyer<out TKey>
    {
        /// <summary>
        /// Gets the help key for the given context.
        /// </summary>
        /// <param name="context">
        /// The context for which the help key is required.
        /// </param>
        /// <returns>
        /// null if no key is available the help context.
        /// </returns>
        [CanBeNull]
        TKey GetHelpKey([NotNull] ContextInfo context);
    }
}
