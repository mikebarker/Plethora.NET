namespace Plethora.Context
{
    public interface IAction
    {
        string ActionName { get; }

        string ActionDescription { get; }

        bool CanExecute { get; }

        void Execute();
    }
}
