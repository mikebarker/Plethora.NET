using System;
using System.Collections.Generic;
using System.Drawing;

namespace Plethora.Context.Action
{
    internal class TemplateActionFactory : IActionFactory
    {
        private readonly Dictionary<string, List<IActionTemplate>> templates = new Dictionary<string, List<IActionTemplate>>();
        private readonly Dictionary<string, List<IMultiActionTemplate>> multiTemplates = new Dictionary<string, List<IMultiActionTemplate>>();

        public void RegisterActionTemplate(IMultiActionTemplate template)
        {
            //Validation
            if (template == null)
                throw new ArgumentNullException("template");


            string contextName = template.ContextName;

            List<IMultiActionTemplate> list;
            if (!multiTemplates.TryGetValue(contextName, out list))
            {
                list = new List<IMultiActionTemplate>();
                multiTemplates.Add(contextName, list);
            }

            list.Add(template);
        }

        public void RegisterActionTemplate(IActionTemplate template)
        {
            //Validation
            if (template == null)
                throw new ArgumentNullException("template");


            string contextName = template.ContextName;

            List<IActionTemplate> list;
            if (!templates.TryGetValue(contextName, out list))
            {
                list = new List<IActionTemplate>();
                templates.Add(contextName, list);
            }

            list.Add(template);
        }

        public void DeregisterActionTemplate(IActionTemplate template)
        {
            //Validation
            if (template == null)
                throw new ArgumentNullException("template");


            List<IActionTemplate> list;
            if (templates.TryGetValue(template.ContextName, out list))
                list.Remove(template);
        }

        public void DeregisterActionTemplate(IMultiActionTemplate template)
        {
            //Validation
            if (template == null)
                throw new ArgumentNullException("template");


            List<IMultiActionTemplate> list;
            if (multiTemplates.TryGetValue(template.ContextName, out list))
                list.Remove(template);
        }

        public IEnumerable<IAction> GetActions(IDictionary<string, ContextInfo[]> contextsByName)
        {
            List<IAction> actions = new List<IAction>();

            foreach (var pair in contextsByName)
            {
                var contextName = pair.Key;
                var contexts = pair.Value;

                if (contexts.Length == 0)
                {
                    //Do nothing
                }
                else if (contexts.Length == 1)
                {
                    List<IActionTemplate> list;
                    if (templates.TryGetValue(contextName, out list))
                    {
                        foreach (var template in list)
                        {
                            var uiTemplate = template as IUiActionTemplate;
                            IAction action = (uiTemplate != null)
                                ? new UiContextAction(uiTemplate, contexts[0])
                                : new ContextAction(template, contexts[0]);

                            actions.Add(action);
                        }
                    }
                }
                else
                {
                    List<IMultiActionTemplate> list;
                    if (multiTemplates.TryGetValue(contextName, out list))
                    {
                        foreach (var template in list)
                        {
                            var uiTemplate = template as IUiMultiActionTemplate;
                            IAction action = (uiTemplate != null)
                                ? new UiMultiContextAction(uiTemplate, contexts)
                                : new MultiContextAction(template, contexts);

                            actions.Add(action);
                        }
                    }
                }
            }

            actions.TrimExcess();
            return actions;
        }


        private class ContextAction : IAction
        {
            #region Fields

            private readonly ContextInfo context;
            private readonly IActionTemplate template;

            #endregion

            #region Constructor

            public ContextAction(IActionTemplate template, ContextInfo context)
            {
                this.context = context;
                this.template = template;
            }

            #endregion

            protected ContextInfo Context
            {
                get { return context; }
            }

            protected IActionTemplate Template
            {
                get { return template; }
            }


            #region Implementation of IAction

            public string ActionName
            {
                get { return this.template.GetActionName(this.context); }
            }

            public bool CanExecute
            {
                get { return this.template.CanExecute(this.context); }
            }

            public void Execute()
            {
                this.template.Execute(this.context);
            }

            #endregion
        }

        private class UiContextAction : ContextAction, IUiAction
        {
            #region Constructor

            public UiContextAction(IUiActionTemplate template, ContextInfo context)
                : base(template, context)
            {
            }

            #endregion

            protected new IUiActionTemplate Template
            {
                get { return (IUiActionTemplate)base.Template; }
            }

            #region Implementation of IUiAction

            public string ActionText
            {
                get { return this.Template.GetActionText(this.Context); }
            }

            public string ActionDescription
            {
                get { return this.Template.GetActionDescription(this.Context); }
            }

            public Image Image
            {
                get { return this.Template.GetImage(this.Context); }
            }

            public string Group
            {
                get { return this.Template.GetGroup(this.Context); }
            }

            public int Rank
            {
                get { return this.Template.GetRank(this.Context); }
            }

            #endregion
        }

        private class MultiContextAction : IAction
        {
            #region Fields

            private readonly IMultiActionTemplate template;
            private readonly ContextInfo[] contexts;

            #endregion

            #region Constructors

            public MultiContextAction(IMultiActionTemplate template, ContextInfo[] contexts)
            {
                this.template = template;
                this.contexts = contexts;
            }

            #endregion

            protected ContextInfo[] Contexts
            {
                get { return contexts; }
            }

            protected IMultiActionTemplate Template
            {
                get { return this.template; }
            }

            #region Implementation of IAction

            public string ActionName
            {
                get { return this.template.GetActionName(this.contexts); }
            }

            public bool CanExecute
            {
                get { return this.template.CanExecute(this.contexts); }
            }

            public void Execute()
            {
                this.template.Execute(this.contexts);
            }

            #endregion
        }

        private class UiMultiContextAction : MultiContextAction, IUiAction
        {
            #region Constructors

            public UiMultiContextAction(IUiMultiActionTemplate template, ContextInfo[] contexts)
                : base(template, contexts)
            {
            }

            #endregion

            protected new IUiMultiActionTemplate Template
            {
                get { return (IUiMultiActionTemplate)base.Template; }
            }

            #region Implementation of IUiAction

            public string ActionText
            {
                get { return this.Template.GetActionText(this.Contexts); }
            }

            public string ActionDescription
            {
                get { return this.Template.GetActionDescription(this.Contexts); }
            }

            public Image Image
            {
                get { return this.Template.GetImage(this.Contexts); }
            }

            public string Group
            {
                get { return this.Template.GetGroup(this.Contexts); }
            }

            public int Rank
            {
                get { return this.Template.GetRank(this.Contexts); }
            }

            #endregion
        }
    }
}