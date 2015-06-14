﻿using System;

namespace Plethora.Context.Action
{
    public abstract class ActionTemplate : IActionTemplate
    {
        private readonly string contextName;

        protected ActionTemplate(
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

        public abstract string GetActionName(ContextInfo context);

        public abstract bool CanExecute(ContextInfo context);

        public abstract void Execute(ContextInfo context);

        public virtual IAction CreateAction(ContextInfo[] contexts)
        {
            if (contexts.Length != 1)
                return null;


            ContextInfo context = contexts[0];

            string actionName = GetActionName(context);
            bool canExecute = CanExecute(context);
            System.Action execute = () => Execute(context);

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
            string actionName,
            bool canExecute,
            System.Action execute)
        {
            //Validation
            if (actionName == null)
                throw new ArgumentNullException("actionName");

            if (execute == null)
                throw new ArgumentNullException("execute");


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
