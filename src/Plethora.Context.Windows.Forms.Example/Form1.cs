using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Plethora.Context.Action;

namespace Plethora.Context.Windows.Forms.Example
{
    public partial class Form1 : Form
    {
        private class ContractContextInfo : ContextInfo
        {
            public ContractContextInfo(long contractId, int rank = 10000)
                : base("Contract", rank, contractId)
            {
            }
        }

        private class InstrumentContextInfo : ContextInfo
        {
            public InstrumentContextInfo(long instrumentId, int rank = 7000)
                : base("Instrument", rank, instrumentId)
            {
            }
        }

        private class PositionContextInfo : ContextInfo
        {
            public PositionContextInfo(Position position, int rank = 8000)
                : base("Position", rank, position)
            {
            }
        }


        private readonly ContextManager contextManager;
        private readonly ActionManager actionManager;

        public Form1()
        {
            InitializeComponent();

            FrmContext contextForm = new FrmContext();
            contextForm.Show();

            contextManager = ContextManager.GlobalInstance;
            actionManager = ActionManager.GlobalInstance;

            contextActionMenuStrip.ContextManager = contextManager;

            contextManager.RegisterProvider(new TextBoxContextProvider(this.textBox1, GetContractContext));
            contextManager.RegisterProvider(new ListViewContextProvider(this.listView1, GetPositionContext));

            //Augments positions with contract and instrument contexts.
            ContextAugmentor positionAugmentor = new ContextAugmentor("Position", positionContext => new ContextInfo[]
                {
                    new ContractContextInfo(((Position)positionContext.Data).ContractID, positionContext.Rank - 100),
                    new InstrumentContextInfo(((Position)positionContext.Data).InstrumentID, positionContext.Rank - 200),
                });
            contextManager.RegisterAugmentor(positionAugmentor);


            //Register actions
            IActionTemplate viewContractAction = new ContextActionTemplate("Contract", c => "View Contract #" + ((long)c.Data));
            actionManager.RegisterActionTemplate(viewContractAction);

            IActionTemplate editContractAction = new ContextActionTemplate("Contract", c => "Edit Contract #" + ((long)c.Data));
            actionManager.RegisterActionTemplate(editContractAction);

            IActionTemplate viewInstrumentAction = new ContextActionTemplate("Instrument", c => "View Instrument #" + ((long)c.Data));
            actionManager.RegisterActionTemplate(viewInstrumentAction);

            IActionTemplate viewMultiInstrumentAction = new MultiContextActionTemplate("Instrument", array => "View All Instruments");
            actionManager.RegisterActionTemplate(viewMultiInstrumentAction);


            contextManager.ContextChanged += contextManager_ContextChanged;

            ActivityItemRegister activityItemRegister = ActivityItemRegister.Instance;
            activityItemRegister.RegisterActivityItem(this.groupBox2);
        }

        private void contextManager_ContextChanged(object sender, EventArgs e)
        {
            var contexts = contextManager.GetContexts();
            var actions = actionManager.GetActions(contexts);

            this.groupBox2.Controls.Clear();
            foreach (var action in actions.OrderByDescending(ActionHelper.GetRank).ThenBy(a => a.ActionName))
            {
                var contextAction = action;

                Button btn = new Button();
                btn.Dock = DockStyle.Top;
                btn.Height = 48;
                btn.Text = contextAction.ActionName;
                btn.Enabled = contextAction.CanExecute;
                btn.Click += delegate { contextAction.Execute(); };

                this.groupBox2.Controls.Add(btn);
            }
        }

        public static IEnumerable<ContextInfo> GetContractContext(TextBox textBox)
        {
            string text = textBox.Text;
            long num;
            if (!long.TryParse(text, out num))
                return null;
            
            return Enumerable.Repeat(new ContractContextInfo(num), 1);
        }

        private static IEnumerable<ContextInfo> GetPositionContext(ListView listView)
        {
            var items = listView.SelectedItems;
            var positions = items.Cast<ListViewItem>().Select(GetPosition).Distinct();
            var positionContexts = positions.Select(datum => new PositionContextInfo(datum)).ToList();
            if (!positionContexts.Any())
                return null;

            return positionContexts;
        }

        private static long GetContractID(ListViewItem item)
        {
            long result;
            long.TryParse(item.SubItems[0].Text, out result);
            return result;
        }

        private static long GetInstrumentID(ListViewItem item)
        {
            long result;
            long.TryParse(item.SubItems[1].Text, out result);
            return result;
        }

        private static Position GetPosition(ListViewItem item)
        {
            return new Position(GetContractID(item), GetInstrumentID(item));
        }
    }


    class Position : IEquatable<Position>
    {
        private readonly long contractId;
        private readonly long instrumentId;

        public Position(long contractId, long instrumentId)
        {
            this.contractId = contractId;
            this.instrumentId = instrumentId;
        }

        public long ContractID
        {
            get { return this.contractId; }
        }

        public long InstrumentID
        {
            get { return this.instrumentId; }
        }

        public override string ToString()
        {
            return string.Format("{{ContractID[{0}], InstrumentID[{1}]}}", contractId, instrumentId);
        }

        public bool Equals(Position other)
        {
            return contractId == other.contractId && instrumentId == other.instrumentId;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Position)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (contractId.GetHashCode()*397) ^ instrumentId.GetHashCode();
            }
        }
    }

    class ContextActionTemplate : ActionTemplate
    {
        private readonly Func<ContextInfo, string> getActionName;

        public ContextActionTemplate(string contextName, Func<ContextInfo, string> getActionName)
            : base (contextName)
        {
            this.getActionName = getActionName;
        }

        protected override string GetActionName(ContextInfo info)
        {
            return getActionName(info);
        }

        protected override bool GetCanExecuteAction(ContextInfo context)
        {
            return true;
        }

        protected override System.Action GetExecuteAction(ContextInfo context)
        {
            return () => MessageBox.Show("Executed: " + GetActionName(context), "Executed Action", MessageBoxButtons.OK);
        }
    }

    class MultiContextActionTemplate : MultiActionTemplate
    {
        private readonly Func<ContextInfo[], string> getActionName;

        public MultiContextActionTemplate(string contextName, Func<ContextInfo[], string> getActionName)
            : base(contextName)
        {
            this.getActionName = getActionName;
        }

        protected override string GetActionName(ContextInfo[] contexts)
        {
            return getActionName(contexts);
        }

        protected override bool GetCanExecuteAction(ContextInfo[] contexts)
        {
            return true;
        }

        protected override System.Action GetExecuteAction(ContextInfo[] contexts)
        {
            return
                () => MessageBox.Show("Executed: " + GetActionName(contexts), "Executed Action", MessageBoxButtons.OK);
        }
    }
}
