using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

using Plethora.Context.Action;

namespace Plethora.Context.Wpf.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WpfCallbackDelay<EventArgs> callbackDelay;

        public MainWindow()
        {
            InitializeComponent();

            this.callbackDelay = new WpfCallbackDelay<EventArgs>(this.ContextManager_ContextChanged, 10);
            ContextManager.GlobalInstance.ContextChanged += this.callbackDelay.Handler;

            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Contract", "View Contract"));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Contract", "Edit Contract"));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("SignedContract", "View Signed Contract", false));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Instrument", "View Instrument"));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Textbox", "Cut"));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Textbox", "Copy"));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Textbox", "Paste"));
        }

        void ContextManager_ContextChanged(object sender, EventArgs e)
        {
            var contexts = ContextManager.GlobalInstance.GetContexts();

            var contextText = contexts
                .Select(context => string.Format("{0} [{1}]", context.Name, context.Data));

            this.ContextTextBox.Text = string.Join("\r\n", contextText);

            var actions = ActionManager.GlobalInstance.GetActions(contexts);

            this.ContextActionStack.Children.Clear();
            foreach (var action in actions.OrderByDescending(ActionHelper.GetRank).ThenBy(a => a.ActionName))
            {
                var contextAction = action;

                Button btn = new Button();
                btn.Height = 48;
                btn.Content = contextAction.ActionName;
                btn.IsEnabled = contextAction.CanExecute;
                btn.Click += delegate { contextAction.Execute(); };

                this.ContextActionStack.Children.Add(btn);
            }
        }


        class GenericActionTemplate : UiActionTemplate
        {
            private readonly string contextName;
            private readonly string actionName;
            private readonly bool canExecute;

            public GenericActionTemplate(string contextName, string actionName)
                : this(contextName, actionName, true)
            {
            }

            public GenericActionTemplate(string contextName, string actionName, bool canExecute)
                : base(contextName)
            {
                this.contextName = contextName;
                this.actionName = actionName;
                this.canExecute = canExecute;
            }

            #region Implementation of IActionTemplate

            public override string GetActionName(ContextInfo context)
            {
                return this.actionName;
            }

            public override bool CanExecute(ContextInfo context)
            {
                return this.canExecute;
            }

            public override void Execute(ContextInfo context)
            {
                MessageBox.Show("Executed " + this.GetActionName(context), "Execute Action", MessageBoxButton.OK);
            }

            #endregion

            #region Implementation of IUiActionTemplate

            public override string GetActionText(ContextInfo context)
            {
                return "Execute " + this.GetActionName(context);
            }

            public override string GetActionDescription(ContextInfo context)
            {
                return "Execute " + this.GetActionName(context) + " [" + context.Data + "]";
            }

            public override object GetImageKey(ContextInfo context)
            {
                return null;
            }

            public override string GetGroup(ContextInfo context)
            {
                return this.contextName;
            }

            public override int GetRank(ContextInfo context)
            {
                return context.Rank;
            }

            #endregion


        }
    }
}
