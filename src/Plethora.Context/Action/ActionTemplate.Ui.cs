using System.Drawing;

namespace Plethora.Context.Action
{
    public abstract class UiActionTemplate : ActionTemplate
    {
        protected UiActionTemplate(string contextName)
            : base(contextName)
        {
        }

        protected abstract string GetDescription(ContextInfo context);

        protected abstract Image GetImage(ContextInfo context);

        protected abstract string GetGroup(ContextInfo context);

        protected abstract int GetRank(ContextInfo context);


        public override IAction CreateAction(ContextInfo[] contexts)
        {
            if (contexts.Length != 1)
                return null;


            ContextInfo context = contexts[0];

            string actionName = GetActionName(context);
            string description = GetDescription(context);
            Image image = GetImage(context);
            string group = GetGroup(context);
            int rank = GetRank(context);
            bool canExecute = GetCanExecuteAction(context);
            System.Action execute = GetExecuteAction(context);

            IAction action = new UiContextAction(actionName, description, image, group, rank, canExecute, execute);
            return action;
        }
    }

    public class UiContextAction : ContextAction, IUiAction, IAction
    {
        private readonly string description;
        private readonly Image image;
        private readonly string group;
        private readonly int rank;

        public UiContextAction(
            string actionName,
            string description,
            Image image,
            string group,
            int rank,
            bool canExecute,
            System.Action execute)
            : base(actionName, canExecute, execute)
        {
            this.description = description;
            this.image = image;
            this.group = group;
            this.rank = rank;
        }

        public string ActionDescription
        {
            get { return this.description; }
        }

        public Image Image
        {
            get { return this.image; }
        }

        public string Group
        {
            get { return this.group; }
        }

        public int Rank
        {
            get { return this.rank; }
        }

    }
}
