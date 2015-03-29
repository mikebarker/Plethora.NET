namespace Plethora.Context.Action
{
    /// <summary>
    /// A template used to create <see cref="IAction"/> instances to operate
    /// on <see cref="ContextInfo"/> items.
    /// </summary>
    /// <remarks>
    /// An <see cref="IActionTemplate"/> is used to dynamically create an instance of 
    /// <see cref="IAction"/> at run-time, according to multiple in-scope contexts with the
    /// same context name.
    /// </remarks>
    public interface IActionTemplate
    {
        string ContextName { get; }

        IAction CreateAction(ContextInfo[] context);
    }
}
