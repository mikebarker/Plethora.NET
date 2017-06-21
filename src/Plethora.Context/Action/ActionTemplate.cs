using System;

using JetBrains.Annotations;

namespace Plethora.Context.Action
{
    public abstract class ActionTemplate : IActionTemplate
    {
        private readonly string contextName;

        protected ActionTemplate(
            [NotNull] string contextName)
        {
            //Validation
            if (contextName == null)
                throw new ArgumentNullException(nameof(contextName));


            this.contextName = contextName;
        }

        public string ContextName
        {
            get { return this.contextName; }
        }

        public abstract string GetActionName(ContextInfo context);

        public abstract bool CanExecute(ContextInfo context);

        public abstract void Execute(ContextInfo context);

        [CanBeNull]
        public virtual IAction CreateAction([NotNull, ItemNotNull] ContextInfo[] contexts)
        {
            if (contexts.Length != 1)
                return null;


            ContextInfo context = contexts[0];

            string actionName = this.GetActionName(context);
            bool canExecute = this.CanExecute(context);
            System.Action execute = () => this.Execute(context);

            IAction action = new ContextAction(actionName, canExecute, execute);
            return action;
        }
    }

    public class ContextAction : IAction
    {
        private readonly string actionName;
        private readonly bool canExecute;
        private readonly System.Action execute;

        public ContextAction(
            [NotNull] string actionName,
            bool canExecute,
            [NotNull] System.Action execute)
        {
            //Validation
            if (actionName == null)
                throw new ArgumentNullException(nameof(actionName));

            if (execute == null)
                throw new ArgumentNullException(nameof(execute));


            this.actionName = actionName;
            this.canExecute = canExecute;
            this.execute = execute;
        }

        public string ActionName
        {
            get { return this.actionName; }
        }

        public bool CanExecute
        {
            get { return this.canExecute; }
        }

        public void Execute()
        {
            this.execute();
        }
    }
}
