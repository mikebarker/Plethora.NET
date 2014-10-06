using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Image = System.Drawing.Image;

namespace Plethora.Context.Wpf.Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private WpfCallbackDelay callbackDelay;

        public MainWindow()
        {
            InitializeComponent();

            callbackDelay = new WpfCallbackDelay(ContextManager_ContextChanged, 10);
            ContextManager.DefaultInstance.ContextChanged += callbackDelay.Handler;
            ContextManager.DefaultInstance.RegisterActionTemplate(new GenericActionTemplate("Contract", "View Contract"));
            ContextManager.DefaultInstance.RegisterActionTemplate(new GenericActionTemplate("Contract", "Edit Contract"));
            ContextManager.DefaultInstance.RegisterActionTemplate(new GenericActionTemplate("SignedContract", "View Signed Contract", false));
            ContextManager.DefaultInstance.RegisterActionTemplate(new GenericActionTemplate("Instrument", "View Instrument"));
            ContextManager.DefaultInstance.RegisterActionTemplate(new GenericActionTemplate("Textbox", "Cut"));
            ContextManager.DefaultInstance.RegisterActionTemplate(new GenericActionTemplate("Textbox", "Copy"));
            ContextManager.DefaultInstance.RegisterActionTemplate(new GenericActionTemplate("Textbox", "Paste"));
        }

        void ContextManager_ContextChanged(object sender, EventArgs e)
        {
            var contexts = ContextManager.DefaultInstance.GetContexts();

            var contextText = contexts
                .Select(context => string.Format("{0} [{1}]", context.Name, context.Data));

            this.ContextTextBox.Text = string.Join("\r\n", contextText);

            var actions = ContextManager.DefaultInstance.GetActions(contexts);

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


        class GenericActionTemplate : IUiActionTemplate
        {
            private readonly string contextName;
            private readonly string actionName;
            private readonly bool canExecute;

            public GenericActionTemplate(string contextName, string actionName)
                : this(contextName, actionName, true)
            {
            }

            public GenericActionTemplate(string contextName, string actionName, bool canExecute)
            {
                this.contextName = contextName;
                this.actionName = actionName;
                this.canExecute = canExecute;
            }

            #region Implementation of IActionTemplate

            public string ContextName
            {
                get { return contextName; }
            }

            public string GetActionName(ContextInfo context)
            {
                return this.actionName;
            }

            public bool CanExecute(ContextInfo context)
            {
                return this.canExecute;
            }

            public void Execute(ContextInfo context)
            {
                MessageBox.Show("Executed " + GetActionName(context), "Execute Action", MessageBoxButton.OK);
            }

            #endregion

            #region Implementation of IUiActionTemplate

            public string GetActionDescription(ContextInfo context)
            {
                return "Execute " + GetActionName(context) + " [" + context.Data + "]";
            }

            public Image GetImage(ContextInfo context)
            {
                return null;
            }

            public string GetGroup(ContextInfo context)
            {
                return this.contextName;
            }

            public int GetRank(ContextInfo context)
            {
                return context.Rank;
            }

            #endregion


        }
    }
}
