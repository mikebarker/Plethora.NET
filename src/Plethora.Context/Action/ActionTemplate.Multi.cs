using System;

namespace Plethora.Context.Action
{
    public abstract class MultiActionTemplate : IActionTemplate
    {
        private readonly string contextName;

        protected MultiActionTemplate(
            string contextName)
        {
            //Validation
            if (contextName == null)
                throw new ArgumentNullException("contextName");


            this.contextName = contextName;
        }

        public string ContextName
        {
            get { return this.contextName; }
        }

        protected abstract string GetActionName(ContextInfo[] context);

        protected abstract bool GetCanExecuteAction(ContextInfo[] context);

        protected abstract System.Action GetExecuteAction(ContextInfo[] context);

        public virtual IAction CreateAction(ContextInfo[] contexts)
        {
            if (contexts.Length < 2)
                return null;

            string actionName = GetActionName(contexts);
            bool canExecute = GetCanExecuteAction(contexts);
            System.Action execute = GetExecuteAction(contexts);

            IAction action = new ContextAction(actionName, canExecute, execute);
            return action;
        }
    }
}
