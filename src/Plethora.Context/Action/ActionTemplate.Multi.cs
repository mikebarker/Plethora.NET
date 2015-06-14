using System;

namespace Plethora.Context.Action
{
    public abstract class MultiActionTemplate : IMultiActionTemplate
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

        public abstract string GetActionName(ContextInfo[] context);

        public abstract bool CanExecute(ContextInfo[] context);

        public abstract void Execute(ContextInfo[] context);

        public virtual IAction CreateAction(ContextInfo[] contexts)
        {
            if (contexts.Length < 2)
                return null;

            string actionName = GetActionName(contexts);
            bool canExecute = CanExecute(contexts);
            System.Action execute = () => Execute(contexts);

            IAction action = new ContextAction(actionName, canExecute, execute);
            return action;
        }
    }
}
