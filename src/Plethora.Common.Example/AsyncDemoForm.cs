using System;
using System.Threading;
using System.Windows.Forms;
using Plethora.ComponentModel;

namespace Plethora.Example
{
    public partial class AsyncDemoForm : Form
    {
        public AsyncDemoForm()
        {
            InitializeComponent();
        }

        private void btnStartAsync_Click(object sender, EventArgs e)
        {
            btnStartAsync.Enabled = false;
            lblExecuting.Visible = true;
            lblComplete.Visible = false;

            Action action = delegate
                {
                    //Some long running task (eg. database access)
                    Thread.Sleep(7000);
                };

            Action onComplete = delegate
                {
                    btnStartAsync.Enabled = true;
                    lblExecuting.Visible = false;
                    lblComplete.Visible = true;
                };

            btnStartAsync.AsyncTask(
                action,
                onComplete);
        }
    }
}
