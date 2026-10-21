using System;

namespace Plethora.Context.Action;

public abstract class ActionTemplate : IActionTemplate
{
    private readonly string contextName;

    protected ActionTemplate(string contextName)
    {
        //Validation
        ArgumentNullException.ThrowIfNull(contextName);

        this.contextName = contextName;
    }

    public string ContextName
    {
        get { return this.contextName; }
    }

    public abstract string GetActionName(ContextInfo context);

    public abstract bool CanExecute(ContextInfo context);

    public abstract void Execute(ContextInfo context);

    public virtual IAction? CreateAction(ContextInfo[] contexts)
    {
        if (contexts.Length != 1)
            return null;


        ContextInfo context = contexts[0];

        string actionName = this.GetActionName(context);
        bool canExecute = this.CanExecute(context);
        void execute() => this.Execute(context);

        IAction action = new ContextAction(actionName, canExecute, execute);
        return action;
    }
}

public class ContextAction : IAction
{
    private readonly string actionName;
    private readonly bool canExecute;
    private readonly System.Action execute;

    public ContextAction(
        string actionName,
        bool canExecute,
        System.Action execute)
    {
        //Validation
        ArgumentNullException.ThrowIfNull(actionName);
        ArgumentNullException.ThrowIfNull(execute);

        this.actionName = actionName;
        this.canExecute = canExecute;
        this.execute = execute;
    }

    public string ActionName
    {
        get { return this.actionName; }
    }

    public bool CanExecute
    {
        get { return this.canExecute; }
    }

    public void Execute()
    {
        this.execute();
    }
}
