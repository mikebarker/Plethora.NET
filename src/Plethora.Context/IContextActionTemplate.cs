namespace Plethora.Context
{
    public interface IContextActionTemplate
    {
        string ContextName { get; }

        string GetActionName(ContextInfo info);

        string GetActionDescription(ContextInfo info);

        bool CanExecute(ContextInfo info);

        void Execute(ContextInfo info);
    }

    public interface IMultiContextActionTemplate
    {
        string ContextName { get; }

        string GetActionName(ContextInfo[] contexts);

        string GetActionDescription(ContextInfo[] contexts);

        bool CanExecute(ContextInfo[] contexts);

        void Execute(ContextInfo[] contexts);
    }
}
