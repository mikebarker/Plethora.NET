using Plethora.Context.Action;
using Plethora.Xaml.Uwp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;

namespace Plethora.Context.Xaml.Uwp.Sample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page, INotifyPropertyChanged
    {
        // Being a field ensures that the reference to the callbackDelay is not garbage collected.
        private readonly CallbackDelay<EventArgs> callbackDelay;

        private int contract;
        private string instrument;
        private string contextText;
        private IEnumerable<IAction> actions;

        public MainPage()
        {
            this.InitializeComponent();

            // By introducing a delay one can avoid multiple hits to the ContextChanged handler.
            // The context will change each time a control receives keyboard focus and when a the focus leaves the control.
            // Therefore as the user moves focus from one control to another the context leaves the first, and then
            // is given to the second. This results in two context changes. Only one is required to re-acquire the actions.
            this.callbackDelay = new CallbackDelay<EventArgs>(this.ContextManager_ContextChanged, TimeSpan.FromMilliseconds(10), this.Dispatcher);
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
        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion


        void ContextManager_ContextChanged(object sender, EventArgs e)
        {
            var contexts = ContextManager.GlobalInstance.GetContexts();

            var contextTexts = contexts
                .Select(context => string.Format("{0} [{1}]", context.Name, context.Data));

            this.ContextText = string.Join(Environment.NewLine, contextTexts);

            var actions = ActionManager.GlobalInstance.GetActions(contexts);

            this.Actions = actions.OrderByDescending(action => ActionHelper.GetRank(action)).ThenBy(a => a.ActionName);
        }

        public int Contract
        {
            get { return this.contract; }
            set
            {
                this.contract = value;
                this.OnPropertyChanged();
            }
        }

        public string Instrument
        {
            get { return this.instrument; }
            set
            {
                this.instrument = value;
                this.OnPropertyChanged();
            }
        }

        public string ContextText
        {
            get { return this.contextText; }
            set
            {
                this.contextText = value;
                this.OnPropertyChanged();
            }
        }

        public IEnumerable<IAction> Actions
        {
            get { return this.actions; }
            set
            {
                this.actions = value;
                this.OnPropertyChanged();
            }
        }


        /// <summary>
        /// A generic action which simply brings up a dialogue box informing the user that the action has been executed.
        /// </summary>
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
                var dialog = new MessageDialog("Executed " + this.GetActionName(context), "Execute Action");
                var task = dialog.ShowAsync();
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
