using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Plethora.Context.Action;

namespace Plethora.Context.Windows.Forms.Example
{
    public partial class FrmContext : Form
    {
        private readonly ContextManager contextManager;
        private readonly ActionManager actionManager;

        public FrmContext()
        {
            InitializeComponent();

            contextManager = ContextManager.GlobalInstance;
            contextManager.ContextChanged += contextManager_ContextChanged;

            actionManager = ActionManager.GlobalInstance;
        }

        protected override void OnClosed(EventArgs e)
        {
            contextManager.ContextChanged -= contextManager_ContextChanged;

            base.OnClosed(e);
        }

        void contextManager_ContextChanged(object sender, EventArgs e)
        {
            RefreshContext();
        }

        private void RefreshContext()
        {
            this.textBox1.Clear();
            this.textBox2.Clear();

            var contexts = contextManager.GetContexts();
            var actions = actionManager.GetActions(contexts);

            foreach (var context in contexts.OrderByDescending(c => c.Rank).ThenBy(c => (c.Data as IComparable) ?? -1))
            {
                string dataString;
                if (context.Data is string)
                {
                    dataString = (string)context.Data;
                }
                else if (context.Data is IEnumerable)
                {
                    bool first = true;
                    StringBuilder sb = new StringBuilder();
                    foreach (var datum in (IEnumerable)context.Data)
                    {
                        if (!first)
                            sb.Append(", ");
                        else
                            first = false;

                        sb.Append(datum.ToString());
                    }
                    dataString = "{{" + sb.ToString() + "}}";
                }
                else
                {
                    dataString = context.Data.ToString();
                }

                this.textBox1.Text += string.Format("{0} - {1}\r\n",
                                                    context.Name,
                                                    dataString);
            }

            foreach (var action in actions)
            {
                textBox2.Text += string.Format("{0}\r\n",
                                               action.ActionName);
            }
        }
    }
}
