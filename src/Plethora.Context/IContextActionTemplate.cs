namespace Plethora.Context
{
    public interface IContextActionTemplate
    {
        string ContextName { get; }

        string GetActionName(ContextInfo info);

        bool CanExecute(ContextInfo info);

        void Execute(ContextInfo info);
    }

    public interface IMultiContextActionTemplate
    {
        string ContextName { get; }

        string GetActionName(ContextInfo[] contexts);

        bool CanExecute(ContextInfo[] contexts);

        void Execute(ContextInfo[] contexts);
    }
}
