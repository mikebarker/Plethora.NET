namespace Plethora.Context
{
    public interface IContextActionTemplate
    {
        string ContextName { get; }

        string GetActionName(ContextInfo context);

        bool CanExecute(ContextInfo context);

        void Execute(ContextInfo context);
    }

    public interface IMultiContextActionTemplate
    {
        string ContextName { get; }

        string GetActionName(ContextInfo[] contexts);

        bool CanExecute(ContextInfo[] contexts);

        void Execute(ContextInfo[] contexts);
    }
}
