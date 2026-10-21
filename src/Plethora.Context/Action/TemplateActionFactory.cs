using System;
using System.Collections.Generic;

namespace Plethora.Context.Action;

internal class TemplateActionFactory : IActionFactory
{
    private readonly Dictionary<string, List<IActionTemplate>> templates = new();
    private readonly Dictionary<string, List<IMultiActionTemplate>> multiTemplates = new();

    public void RegisterActionTemplate(IMultiActionTemplate template)
    {
        //Validation
        ArgumentNullException.ThrowIfNull(template);

        string contextName = template.ContextName;

        if (!this.multiTemplates.TryGetValue(contextName, out var list))
        {
            list = new();
            this.multiTemplates.Add(contextName, list);
        }

        list.Add(template);
    }

    public void RegisterActionTemplate(IActionTemplate template)
    {
        //Validation
        ArgumentNullException.ThrowIfNull(template);


        string contextName = template.ContextName;

        if (!this.templates.TryGetValue(contextName, out var list))
        {
            list = new();
            this.templates.Add(contextName, list);
        }

        list.Add(template);
    }

    public void DeregisterActionTemplate(IActionTemplate template)
    {
        //Validation
        ArgumentNullException.ThrowIfNull(template);

        if (this.templates.TryGetValue(template.ContextName, out var list))
        {
            list.Remove(template);
        }
    }

    public void DeregisterActionTemplate(IMultiActionTemplate template)
    {
        //Validation
        ArgumentNullException.ThrowIfNull(template);

        if (this.multiTemplates.TryGetValue(template.ContextName, out var list))
        {
            list.Remove(template);
        }
    }

    public IEnumerable<IAction> GetActions(IDictionary<string, ContextInfo[]> contextsByName)
    {
        List<IAction> actions = new();

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
                if (this.templates.TryGetValue(contextName, out var list))
                {
                    foreach (var template in list)
                    {
                        IAction action = (template is IUiActionTemplate uiTemplate)
                            ? new UiContextAction(uiTemplate, contexts[0])
                            : new ContextAction(template, contexts[0]);

                        actions.Add(action);
                    }
                }
            }
            else
            {
                if (this.multiTemplates.TryGetValue(contextName, out var list))
                {
                    foreach (var template in list)
                    {
                        IAction action = (template is IUiMultiActionTemplate uiTemplate)
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


    private class ContextAction(IActionTemplate template, ContextInfo context) : IAction
    {

        #region Constructor

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
            get { return template.GetActionName(context); }
        }

        public bool CanExecute
        {
            get { return template.CanExecute(context); }
        }

        public void Execute()
        {
            template.Execute(context);
        }

        #endregion
    }

    private class UiContextAction(IUiActionTemplate template, ContextInfo context) : ContextAction(template, context), IUiAction
    {
        protected new IUiActionTemplate Template
        {
            get { return (IUiActionTemplate)base.Template; }
        }

        #region Implementation of IUiAction

        public string ActionText
        {
            get { return this.Template.GetActionText(this.Context); }
        }

        public string? ActionDescription
        {
            get { return this.Template.GetActionDescription(this.Context); }
        }

        public object? ImageKey
        {
            get { return this.Template.GetImageKey(this.Context); }
        }

        public string? Group
        {
            get { return this.Template.GetGroup(this.Context); }
        }

        public int Rank
        {
            get { return this.Template.GetRank(this.Context); }
        }

        #endregion
    }

    private class MultiContextAction(IMultiActionTemplate template, ContextInfo[] contexts) : IAction
    {
        protected ContextInfo[] Contexts
        {
            get { return contexts; }
        }

        protected IMultiActionTemplate Template
        {
            get { return template; }
        }

        #region Implementation of IAction

        public string ActionName
        {
            get { return template.GetActionName(contexts); }
        }

        public bool CanExecute
        {
            get { return template.CanExecute(contexts); }
        }

        public void Execute()
        {
            template.Execute(contexts);
        }

        #endregion
    }

    private class UiMultiContextAction(IUiMultiActionTemplate template, ContextInfo[] contexts) : MultiContextAction(template, contexts), IUiAction
    {
        protected new IUiMultiActionTemplate Template
        {
            get { return (IUiMultiActionTemplate)base.Template; }
        }

        #region Implementation of IUiAction

        public string ActionText
        {
            get { return this.Template.GetActionText(this.Contexts); }
        }

        public string? ActionDescription
        {
            get { return this.Template.GetActionDescription(this.Contexts); }
        }

        public object? ImageKey
        {
            get { return this.Template.GetImageKey(this.Contexts); }
        }

        public string? Group
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