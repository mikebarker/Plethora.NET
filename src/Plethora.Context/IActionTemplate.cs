namespace Plethora.Context
{
    /// <summary>
    /// A template used to create <see cref="IAction"/> instances to operate on
    /// a single <see cref="ContextInfo"/> item.
    /// </summary>
    /// <remarks>
    /// An <see cref="IActionTemplate"/> is used to dynamically create an instance of 
    /// <see cref="IAction"/> at run-time, according to a single in-scope context.
    /// </remarks>
    /// <example>
    /// An <see cref="IActionTemplate"/> may be defined to view the detail of a single
    /// trade when selected in a grid.
    /// </example>
    /// <seealso cref="IUiActionTemplate"/>
    /// <seealso cref="IMultiActionTemplate"/>
    public interface IActionTemplate
    {
        string ContextName { get; }

        string GetActionName(ContextInfo context);

        bool CanExecute(ContextInfo context);

        void Execute(ContextInfo context);
    }

    /// <summary>
    /// A template used to create <see cref="IAction"/> instances to operate on
    /// multiple <see cref="ContextInfo"/> items.
    /// </summary>
    /// <remarks>
    /// An <see cref="IMultiActionTemplate"/> is used to dynamically create an instance of 
    /// <see cref="IAction"/> at run-time, according to multiple in-scope contexts with the
    /// same context name.
    /// </remarks>
    /// <example>
    /// An <see cref="IMultiActionTemplate"/> may be cancel multiple trades selected within
    /// a grid.
    /// </example>
    /// <seealso cref="IUiMultiActionTemplate"/>
    /// <seealso cref="IActionTemplate"/>
    public interface IMultiActionTemplate
    {
        string ContextName { get; }

        string GetActionName(ContextInfo[] contexts);

        bool CanExecute(ContextInfo[] contexts);

        void Execute(ContextInfo[] contexts);
    }
}
