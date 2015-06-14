using System.Drawing;

namespace Plethora.Context.Action
{
    public abstract class MultiUiActionTemplate : MultiActionTemplate, IUiMultiActionTemplate
    {
        protected MultiUiActionTemplate(string contextName)
            : base(contextName)
        {
        }

        public abstract string GetActionText(ContextInfo[] contexts);

        public abstract string GetActionDescription(ContextInfo[] contexts);

        public abstract Image GetImage(ContextInfo[] contexts);

        public abstract string GetGroup(ContextInfo[] contexts);

        public abstract int GetRank(ContextInfo[] contexts);


        public override IAction CreateAction(ContextInfo[] contexts)
        {
            if (contexts.Length != 1)
                return null;


            string actionName = GetActionName(contexts);
            string text = GetActionText(contexts);
            string description = GetActionDescription(contexts);
            Image image = GetImage(contexts);
            string group = GetGroup(contexts);
            int rank = GetRank(contexts);
            bool canExecute = CanExecute(contexts);
            System.Action execute = () => Execute(contexts);

            IAction action = new UiContextAction(actionName, text, description, image, group, rank, canExecute, execute);
            return action;
        }
    }
}
