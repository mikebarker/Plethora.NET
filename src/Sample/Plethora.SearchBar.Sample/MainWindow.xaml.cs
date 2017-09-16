using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

using Plethora.SearchBar.Definitions;
using Plethora.SearchBar.Definitions.WellKnown;
using Plethora.SearchBar.ParseTree;

namespace Plethora.SearchBar.Sample
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private IEnumerable<Node> nodes;

        public MainWindow()
        {
            InitializeComponent();
        }

        private string searchText;

        public string SearchText
        {
            get
            {
                return this.searchText;
            }
            set
            {
                this.searchText = value;
                this.OnPropertyChanged();

                this.EvaluateRegex();
            }
        }

        private void EvaluateRegex()
        {
            this.Nodes = null;

            IEnumerable<EntityNode> entityNodes = this.SearchBarParser.Parse(this.SearchText);

            this.Nodes = entityNodes;
        }

        private SearchBarParser searchBarParser;

        private SearchBarParser SearchBarParser
        {
            get
            {
                if (this.searchBarParser == null)
                {
                    FieldDefinition tradeDateField = new DateFieldDefinition("trade_date", new[] {"TradeDate", "TD"});
                    FieldDefinition settlementDateField = new DateFieldDefinition("settlement_date", new[] { "SettlementDate", "SettleDate", "SD"});
                    EntityDefinition tradeEntity = new EntityDefinition("trade", new[] {"trade", "T"}, new[] {tradeDateField, settlementDateField});

                    this.searchBarParser = new SearchBarParser(new[] {tradeEntity});
                }

                return this.searchBarParser;
            }
        }

        public IEnumerable<Node> Nodes
        {
            get { return this.nodes; }
            set
            {
                this.nodes = value;
                this.OnPropertyChanged();
            }
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
            var handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            this.OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
