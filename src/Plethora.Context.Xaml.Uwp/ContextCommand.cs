using JetBrains.Annotations;
using Plethora.Context.Action;
using System;
using System.Windows.Input;

namespace Plethora.Context
{
    public class ContextCommand : ICommand, IAction
    {
        private readonly IAction action;

        public ContextCommand([NotNull] IAction action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));


            this.action = action;
        }

        [NotNull]
        protected IAction Action
        {
            get { return this.action; }
        }

        #region Implementation of ICommand

        bool ICommand.CanExecute(object parameter)
        {
            return this.CanExecute;
        }

        event EventHandler ICommand.CanExecuteChanged
        {
            add { }
            remove { }
        }

        void ICommand.Execute(object parameter)
        {
            this.Execute();
        }

        #endregion

        #region Implementation of IAction

        public string ActionName
        {
            get { return this.action.ActionName; }
        }

        public bool CanExecute
        {
            get { return this.action.CanExecute; }
        }

        public void Execute()
        {
            this.action.Execute();
        }

        #endregion
    }

    public class UiContextCommand : ContextCommand, ICommand, IUiAction
    {
        public UiContextCommand([NotNull] IUiAction action)
            : base(action)
        {
        }

        #region Implementation of IUiAction

        public string ActionText
        {
            get { return ((IUiAction)this.Action).ActionText; }
        }

        public string ActionDescription
        {
            get { return ((IUiAction)this.Action).ActionDescription; }
        }

        public object ImageKey
        {
            get { return ((IUiAction)this.Action).ImageKey; }
        }

        public string Group
        {
            get { return ((IUiAction)this.Action).Group; }
        }

        public int Rank
        {
            get { return ((IUiAction)this.Action).Rank; }
        }

        #endregion
    }

    public static class ContextCommandHelper
    {
        [NotNull]
        public static ContextCommand AsCommand([NotNull] this IAction action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));


            if (action is IUiAction)
                return new UiContextCommand((IUiAction)action);
            else
                return new ContextCommand(action);
        }

        [NotNull]
        public static ContextCommand AsCommand([NotNull] this IUiAction uiAction)
        {
            if (uiAction == null)
                throw new ArgumentNullException(nameof(uiAction));


            return new UiContextCommand(uiAction);
        }
    }
}
