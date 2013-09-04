using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

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

        public Form1()
        {
            InitializeComponent();

            FrmContext contextForm = new FrmContext();
            contextForm.Show();

            contextManager = ContextManager.DefaultInstance;

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
            ContextActionTemplate viewContractAction = new ContextActionTemplate("Contract", c => "View Contract #" + ((long)c.Data));
            contextManager.RegisterActionTemplate(viewContractAction);

            ContextActionTemplate editContractAction = new ContextActionTemplate("Contract", c => "Edit Contract #" + ((long)c.Data));
            contextManager.RegisterActionTemplate(editContractAction);

            ContextActionTemplate viewInstrumentAction = new ContextActionTemplate("Instrument", c => "View Instrument #" + ((long)c.Data));
            contextManager.RegisterActionTemplate(viewInstrumentAction);

            IMultiActionTemplate viewMultiInstrumentAction = new MultiContextActionTemplate("Instrument", array => "View All Instruments");
            contextManager.RegisterActionTemplate(viewMultiInstrumentAction);
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
            if (!positions.Any())
                return null;

            return positions.Select(datum => new PositionContextInfo(datum));
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

    class ContextActionTemplate : IActionTemplate
    {
        private readonly string contextName;
        private readonly Func<ContextInfo, string> getActionName;

        public ContextActionTemplate(string contextName, Func<ContextInfo, string> getActionName)
        {
            this.contextName = contextName;
            this.getActionName = getActionName;
        }

        public string ContextName { get { return this.contextName; } }

        public string GetActionName(ContextInfo info)
        {
            return getActionName(info);
        }

        public bool CanExecute(ContextInfo info)
        {
            return true;
        }

        public void Execute(ContextInfo info)
        {
        }
    }

    class MultiContextActionTemplate : IMultiActionTemplate
    {
        private readonly string contextName;
        private readonly Func<ContextInfo[], string> getActionName;

        public MultiContextActionTemplate(string contextName, Func<ContextInfo[], string> getActionName)
        {
            this.contextName = contextName;
            this.getActionName = getActionName;
        }

        public string ContextName { get { return this.contextName; } }

        public string GetActionName(ContextInfo[] info)
        {
            return getActionName(info);
        }

        public bool CanExecute(ContextInfo[] info)
        {
            return true;
        }

        public void Execute(ContextInfo[] info)
        {
        }
    }
}