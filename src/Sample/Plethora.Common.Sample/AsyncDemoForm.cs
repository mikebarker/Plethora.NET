using System;
using System.Threading;
using System.Windows.Forms;

using Plethora.ComponentModel;

namespace Plethora.Sample
{
    public partial class AsyncDemoForm : Form
    {
        public AsyncDemoForm()
        {
            this.InitializeComponent();
        }

        private void btnStartAsync_Click(object sender, EventArgs e)
        {
            this.btnStartAsync.Enabled = false;
            this.lblExecuting.Visible = true;
            this.lblComplete.Visible = false;

            Action action = delegate
                {
                    //Some long running task (eg. database access)
                    Thread.Sleep(7000);
                };

            Action onComplete = delegate
                {
                    this.btnStartAsync.Enabled = true;
                    this.lblExecuting.Visible = false;
                    this.lblComplete.Visible = true;
                };

            this.btnStartAsync.AsyncTask(
                action,
                onComplete);
        }
    }
}
