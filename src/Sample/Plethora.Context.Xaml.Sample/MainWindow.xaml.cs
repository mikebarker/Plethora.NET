using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;

using Plethora.Context.Action;
using Plethora.Context.Help;
using Plethora.Context.Help.Factory;
using Plethora.Context.Help.HelpDocuments;
using Plethora.Context.Help.LocalFileSystem;
using Plethora.Xaml;

namespace Plethora.Context.Xaml.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        // ReSharper disable once PrivateFieldCanBeConvertedToLocalVariable
        // Being a field ensures that the reference to the callbackDelay is not garbage collected.
        private readonly CallbackDelay<EventArgs> callbackDelay;

        private int contract;
        private string instrument;
        private string contextText;
        private IEnumerable<IAction> actions;

        private FlowDocument defaultFlowDocument;

        public MainWindow()
        {
            InitializeComponent();

            // By introducing a delay one can avoid multiple hits to the ContextChanged handler.
            // The context will change each time a control receives keyboard focus and when a the focus leaves the control.
            // Therefore as the user moves focus from one control to another the context leaves the first, and then
            // is given to the second. This results in two context changes. Only one is required to re-acquire the actions.
            this.callbackDelay = new CallbackDelay<EventArgs>(this.ContextManager_ContextChanged, 10);
            ContextManager.GlobalInstance.ContextChanged += this.callbackDelay.Handler;

            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Contract", "View Contract"));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Contract", "Edit Contract"));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("SignedContract", "View Signed Contract", false));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Instrument", "View Instrument"));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Textbox", "Cut"));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Textbox", "Copy"));
            ActionManager.GlobalInstance.RegisterActionTemplate(new GenericActionTemplate("Textbox", "Paste"));

            IHelpKeyer<string> keyer = new LocalFileSystemHelpKeyer(@".", "*.rtf");
            IHelpAccessor<string, string> accessor = new LocalFileSystemHelpAccessor();
            IHelpDocumentCreator<string, string> creator = new RtfHelpDocumentCreator<string>();

            IHelpFactory helpFactory = new HelpFactory<string, string>(
                keyer,
                accessor,
                creator);

            HelpManager.GlobalInstance.RegisterFactory(helpFactory);

            this.Contract = 12345;
            this.Instrument = "VOD.L";

            Paragraph paragraph = new Paragraph();
            paragraph.Foreground = Brushes.DarkGray;
            paragraph.FontStyle = FontStyles.Italic;
            paragraph.Inlines.Add("Press F1 for help.");
            this.defaultFlowDocument = new FlowDocument(paragraph);

            this.HelpRichTextBox.Document = this.defaultFlowDocument;
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

        private void MainWindow_OnKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.F1)
            {
                IEnumerable<ContextInfo> contexts = ContextManager.GlobalInstance.GetContexts();

                IEnumerable<IHelpDocument> documents = HelpManager.GlobalInstance.GetHelpDocuments(contexts);


                FlowDocument flowDocument = this.defaultFlowDocument;

                IHelpDocument doc = documents.FirstOrDefault();
                object data = null;
                if (doc != null)
                {
                    data = doc.Data;
                    if (data is string)
                    {
                        Paragraph paragraph = new Paragraph();
                        paragraph.Inlines.Add((string)data);

                        flowDocument = new FlowDocument(paragraph);
                    }
                }

                this.HelpRichTextBox.Document = flowDocument;
            }
        }
    }
}
