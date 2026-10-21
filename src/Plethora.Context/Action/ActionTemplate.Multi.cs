using System;

namespace Plethora.Context.Action;

public abstract class MultiActionTemplate : IMultiActionTemplate
{
    private readonly string contextName;

    protected MultiActionTemplate(string contextName)
    {
        //Validation
        ArgumentNullException.ThrowIfNull(contextName);

        this.contextName = contextName;
    }

    public string ContextName
    {
        get { return this.contextName; }
    }

    public abstract string GetActionName(ContextInfo[] context);

    public abstract bool CanExecute(ContextInfo[] context);

    public abstract void Execute(ContextInfo[] context);

    public virtual IAction? CreateAction(ContextInfo[] contexts)
    {
        if (contexts.Length < 2)
            return null;

        string actionName = this.GetActionName(contexts);
        bool canExecute = this.CanExecute(contexts);
        void execute() => this.Execute(contexts);

        IAction action = new ContextAction(actionName, canExecute, execute);
        return action;
    }
}
