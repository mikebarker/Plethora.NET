namespace Plethora.Context
{
    /// <summary>
    /// A template used to create <see cref="IAction"/> instances to operate on
    /// a single <see cref="ContextInfo"/> item.
    /// </summary>
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
