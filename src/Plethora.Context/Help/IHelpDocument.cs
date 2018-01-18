namespace Plethora.Context.Help
{
    /// <summary>
    /// A document containing the data for the help for the given context.
    /// </summary>
    public interface IHelpDocument
    {
        object Data { get; }
    }

    /// <summary>
    /// A document containing the data for the help for the given context.
    /// </summary>
    public interface IHelpDocument<out TData> : IHelpDocument
    {
        new TData Data { get; }
    }
}
