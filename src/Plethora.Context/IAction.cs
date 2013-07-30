namespace Plethora.Context
{
    public interface IAction
    {
        string ActionName { get; }

        bool CanExecute { get; }

        void Execute();
    }
}
