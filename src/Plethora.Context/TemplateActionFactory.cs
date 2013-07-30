using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Plethora.Context
{
    internal class TemplateActionFactory : IContextActionFactory
    {
        private readonly Dictionary<string, List<IContextActionTemplate>> templates = new Dictionary<string, List<IContextActionTemplate>>();
        private readonly Dictionary<string, List<IMultiContextActionTemplate>> multiTemplates = new Dictionary<string, List<IMultiContextActionTemplate>>();

        public void RegisterActionTemplate(IMultiContextActionTemplate template)
        {
            string contextName = template.ContextName;

            List<IMultiContextActionTemplate> list;
            if (!multiTemplates.TryGetValue(contextName, out list))
            {
                list = new List<IMultiContextActionTemplate>();
                multiTemplates.Add(contextName, list);
            }

            list.Add(template);
        }

        public void RegisterActionTemplate(IContextActionTemplate template)
        {
            string contextName = template.ContextName;

            List<IContextActionTemplate> list;
            if (!templates.TryGetValue(contextName, out list))
            {
                list = new List<IContextActionTemplate>();
                templates.Add(contextName, list);
            }

            list.Add(template);
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
                    List<IContextActionTemplate> list;
                    if (templates.TryGetValue(contextName, out list))
                    {
                        foreach (var template in list)
                        {
                            var uiTemplate = template as IUiContextActionTemplate;
                            IAction action = (uiTemplate != null)
                                ? new UiContextAction(uiTemplate, contexts.Single())
                                : new ContextAction(template, contexts.Single());

                            actions.Add(action);
                        }
                    }
                }
                else
                {
                    List<IMultiContextActionTemplate> list;
                    if (multiTemplates.TryGetValue(contextName, out list))
                    {
                        foreach (var template in list)
                        {
                            var uiTemplate = template as IUiMultiContextActionTemplate;
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
            private readonly IContextActionTemplate template;

            #endregion

            #region Constructor

            public ContextAction(IContextActionTemplate template, ContextInfo context)
            {
                this.context = context;
                this.template = template;
            }

            #endregion

            protected ContextInfo Context
            {
                get { return context; }
            }

            protected IContextActionTemplate Template
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

            public UiContextAction(IUiContextActionTemplate template, ContextInfo context)
                : base(template, context)
            {
            }

            #endregion

            protected new IUiContextActionTemplate Template
            {
                get { return (IUiContextActionTemplate)base.Template; }
            }

            #region Implementation of IUiAction

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

            private readonly IMultiContextActionTemplate template;
            private readonly ContextInfo[] contexts;

            #endregion

            #region Constructors

            public MultiContextAction(IMultiContextActionTemplate template, ContextInfo[] contexts)
            {
                this.template = template;
                this.contexts = contexts;
            }

            #endregion

            protected ContextInfo[] Contexts
            {
                get { return contexts; }
            }

            protected IMultiContextActionTemplate Template
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

            public UiMultiContextAction(IUiMultiContextActionTemplate template, ContextInfo[] contexts)
                : base(template, contexts)
            {
            }

            #endregion

            protected new IUiMultiContextActionTemplate Template
            {
                get { return (IUiMultiContextActionTemplate)base.Template; }
            }

            #region Implementation of IUiAction

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