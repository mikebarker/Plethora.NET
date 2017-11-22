using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Plethora.Context.Action;

namespace Plethora.Context.Xaml.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private XamlCallbackDelay<EventArgs> callbackDelay;
        private int contract;
        private string instrument;
        private string contextText;
        private IEnumerable<IAction> actions;


        public MainWindow()
        {
            InitializeComponent();

            this.callbackDelay = new XamlCallbackDelay<EventArgs>(this.ContextManager_ContextChanged, 10);
            ContextManager.GlobalInstance.ContextChanged += this.callbackDelay.Handler;

            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Contract", "View Contract"));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Contract", "Edit Contract"));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("SignedContract", "View Signed Contract", false));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Instrument", "View Instrument"));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Textbox", "Cut"));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Textbox", "Copy"));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Textbox", "Paste"));

            this.Contract = 12345;
            this.Instrument = "VOD.L";
        }

        #region PropertyChanged Event

        /// <summary>
        /// Raised when a property has changed.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        protected virtual void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        void ContextManager_ContextChanged(object sender, EventArgs e)
        {
            var contexts = ContextManager.GlobalInstance.GetContexts();

            var contextTexts = contexts
                .Select(context => string.Format("{0} [{1}]", context.Name, context.Data));

            this.ContextText = string.Join("\r\n", contextTexts);

            var actions = ActionManager.GlobalInstance.GetActions(contexts);

            this.Actions = actions.OrderByDescending(action => ActionHelper.GetRank(action)).ThenBy(a => a.ActionName);
        }

        public int Contract
        {
            get { return this.contract; }
            set
            {
                this.contract = value;
                this.OnPropertyChanged(nameof(this.Contract));
            }
        }

        public string Instrument
        {
            get { return this.instrument; }
            set
            {
                this.instrument = value;
                this.OnPropertyChanged(nameof(this.Instrument));
            }
        }

        public string ContextText
        {
            get { return this.contextText; }
            set
            {
                this.contextText = value;
                this.OnPropertyChanged(nameof(this.ContextText));
            }
        }

        public IEnumerable<IAction> Actions
        {
            get { return this.actions; }
            set
            {
                this.actions = value;
                this.OnPropertyChanged(nameof(this.Actions));
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
