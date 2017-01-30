namespace Plethora.Context.Action
{
    public abstract class UiActionTemplate : ActionTemplate, IUiActionTemplate
    {
        protected UiActionTemplate(string contextName)
            : base(contextName)
        {
        }

        public abstract string GetActionText(ContextInfo context);

        public abstract string GetActionDescription(ContextInfo context);

        public abstract object GetImageKey(ContextInfo context);

        public abstract string GetGroup(ContextInfo context);

        public abstract int GetRank(ContextInfo context);


        public override IAction CreateAction(ContextInfo[] contexts)
        {
            if (contexts.Length != 1)
                return null;


            ContextInfo context = contexts[0];

            string actionName = GetActionName(context);
            string text = GetActionText(context);
            string description = GetActionDescription(context);
            object imageKey = GetImageKey(context);
            string group = GetGroup(context);
            int rank = GetRank(context);
            bool canExecute = CanExecute(context);
            System.Action execute = () => Execute(context);

            IAction action = new UiContextAction(actionName, text, description, imageKey, group, rank, canExecute, execute);
            return action;
        }
    }

    public class UiContextAction : ContextAction, IUiAction, IAction
    {
        private readonly string text;
        private readonly string description;
        private readonly object imageKey;
        private readonly string group;
        private readonly int rank;

        public UiContextAction(
            string actionName,
            string text,
            string description,
            object imageKey,
            string group,
            int rank,
            bool canExecute,
            System.Action execute)
            : base(actionName, canExecute, execute)
        {
            this.text = text;
            this.description = description;
            this.imageKey = imageKey;
            this.group = group;
            this.rank = rank;
        }

        public string ActionText
        {
            get { return this.text; }
        }

        public string ActionDescription
        {
            get { return this.description; }
        }

        public object ImageKey
        {
            get { return this.imageKey; }
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
