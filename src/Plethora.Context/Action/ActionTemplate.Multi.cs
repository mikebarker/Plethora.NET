using System;

using JetBrains.Annotations;

namespace Plethora.Context.Action
{
    public abstract class MultiActionTemplate : IMultiActionTemplate
    {
        private readonly string contextName;

        protected MultiActionTemplate(
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

        public abstract string GetActionName(ContextInfo[] context);

        public abstract bool CanExecute(ContextInfo[] context);

        public abstract void Execute(ContextInfo[] context);

        [CanBeNull]
        public virtual IAction CreateAction([NotNull, ItemNotNull] ContextInfo[] contexts)
        {
            if (contexts.Length < 2)
                return null;

            string actionName = this.GetActionName(contexts);
            bool canExecute = this.CanExecute(contexts);
            System.Action execute = () => this.Execute(contexts);

            IAction action = new ContextAction(actionName, canExecute, execute);
            return action;
        }
    }
}
