using System;
using System.Windows.Forms;

namespace Plethora.Windows.Forms.Sample
{
    public partial class MDIParent1 : Form
    {
        public MDIParent1()
        {
            this.InitializeComponent();

            this.ShowNewForm(this, EventArgs.Empty);
        }

        private void ShowNewForm(object sender, EventArgs e)
        {
            Form childForm = new Form1();
            childForm.MdiParent = this;
            childForm.Show();
        }
    }
}
