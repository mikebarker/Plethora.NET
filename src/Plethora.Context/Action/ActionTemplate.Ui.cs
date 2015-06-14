﻿using System.Drawing;

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

        public abstract Image GetImage(ContextInfo context);

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
            Image image = GetImage(context);
            string group = GetGroup(context);
            int rank = GetRank(context);
            bool canExecute = CanExecute(context);
            System.Action execute = () => Execute(context);

            IAction action = new UiContextAction(actionName, text, description, image, group, rank, canExecute, execute);
            return action;
        }
    }

    public class UiContextAction : ContextAction, IUiAction, IAction
    {
        private readonly string text;
        private readonly string description;
        private readonly Image image;
        private readonly string group;
        private readonly int rank;

        public UiContextAction(
            string actionName,
            string text,
            string description,
            Image image,
            string group,
            int rank,
            bool canExecute,
            System.Action execute)
            : base(actionName, canExecute, execute)
        {
            this.text = text;
            this.description = description;
            this.image = image;
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
