using System;
using System.Drawing;
using System.Linq;
using System.Windows;

namespace Plethora.Context.Wpf.Example
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            ContextManager.DefaultInstance.ContextChanged += DefaultInstance_ContextChanged;
            ContextManager.DefaultInstance.RegisterActionTemplate(new GenericActionTemplate("Contract", "View Contract"));
            ContextManager.DefaultInstance.RegisterActionTemplate(new GenericActionTemplate("Contract", "Edit Contract"));
            ContextManager.DefaultInstance.RegisterActionTemplate(new GenericActionTemplate("SignedContract", "View Signed Contract", false));
            ContextManager.DefaultInstance.RegisterActionTemplate(new GenericActionTemplate("Instrument", "View Instrument"));
        }

        void DefaultInstance_ContextChanged(object sender, EventArgs e)
        {
            var contextText = ContextManager.DefaultInstance
                .GetContexts()
                .Select(context => string.Format("{0} [{1}]", context.Name, context.Data));

            this.ContextTextBox.Text = string.Join("\r\n", contextText);
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
                return this.actionName + " [" + context.Data + "]";
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
                return "Execute " + GetActionName(context);
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
                return 0;
            }

            #endregion


        }
    }
}
