using JetBrains.Annotations;

namespace Plethora.Context.Action
{
    public abstract class MultiUiActionTemplate : MultiActionTemplate, IUiMultiActionTemplate
    {
        protected MultiUiActionTemplate([NotNull] string contextName)
            : base(contextName)
        {
        }

        public abstract string GetActionText(ContextInfo[] contexts);

        public abstract string GetActionDescription(ContextInfo[] contexts);

        public abstract object GetImageKey(ContextInfo[] contexts);

        public abstract string GetGroup(ContextInfo[] contexts);

        public abstract int GetRank(ContextInfo[] contexts);


        public override IAction CreateAction(ContextInfo[] contexts)
        {
            if (contexts.Length != 1)
                return null;


            string actionName = this.GetActionName(contexts);
            string text = this.GetActionText(contexts);
            string description = this.GetActionDescription(contexts);
            object imageKey = this.GetImageKey(contexts);
            string group = this.GetGroup(contexts);
            int rank = this.GetRank(contexts);
            bool canExecute = this.CanExecute(contexts);
            System.Action execute = () => this.Execute(contexts);

            IAction action = new UiContextAction(actionName, text, description, imageKey, group, rank, canExecute, execute);
            return action;
        }
    }
}
