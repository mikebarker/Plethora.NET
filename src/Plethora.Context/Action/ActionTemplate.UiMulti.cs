using System.Drawing;

namespace Plethora.Context.Action
{
    public abstract class MultiUiActionTemplate : MultiActionTemplate
    {
        protected MultiUiActionTemplate(string contextName)
            : base(contextName)
        {
        }

        protected abstract string GetDescription(ContextInfo[] contexts);

        protected abstract Image GetImage(ContextInfo[] contexts);

        protected abstract string GetGroup(ContextInfo[] contexts);

        protected abstract int GetRank(ContextInfo[] contexts);


        public override IAction CreateAction(ContextInfo[] contexts)
        {
            if (contexts.Length != 1)
                return null;


            string actionName = GetActionName(contexts);
            string description = GetDescription(contexts);
            Image image = GetImage(contexts);
            string group = GetGroup(contexts);
            int rank = GetRank(contexts);
            bool canExecute = GetCanExecuteAction(contexts);
            System.Action execute = GetExecuteAction(contexts);

            IAction action = new UiContextAction(actionName, description, image, group, rank, canExecute, execute);
            return action;
        }
    }
}
