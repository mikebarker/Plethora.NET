namespace Plethora.Example
{
    partial class AsyncDemoForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.btnStartAsync = new System.Windows.Forms.Button();
            this.lblExecuting = new System.Windows.Forms.Label();
            this.lblComplete = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnStartAsync
            // 
            this.btnStartAsync.Location = new System.Drawing.Point(12, 23);
            this.btnStartAsync.Name = "btnStartAsync";
            this.btnStartAsync.Size = new System.Drawing.Size(260, 30);
            this.btnStartAsync.TabIndex = 0;
            this.btnStartAsync.Text = "Start AsyncTask";
            this.btnStartAsync.UseVisualStyleBackColor = true;
            this.btnStartAsync.Click += new System.EventHandler(this.btnStartAsync_Click);
            // 
            // lblExecuting
            // 
            this.lblExecuting.AutoSize = true;
            this.lblExecuting.Location = new System.Drawing.Point(12, 91);
            this.lblExecuting.Name = "lblExecuting";
            this.lblExecuting.Size = new System.Drawing.Size(90, 13);
            this.lblExecuting.TabIndex = 2;
            this.lblExecuting.Text = "Task Executing...";
            this.lblExecuting.Visible = false;
            // 
            // lblComplete
            // 
            this.lblComplete.AutoSize = true;
            this.lblComplete.Location = new System.Drawing.Point(12, 130);
            this.lblComplete.Name = "lblComplete";
            this.lblComplete.Size = new System.Drawing.Size(81, 13);
            this.lblComplete.TabIndex = 3;
            this.lblComplete.Text = "Task Complete!";
            this.lblComplete.Visible = false;
            // 
            // AsyncDemoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.lblComplete);
            this.Controls.Add(this.lblExecuting);
            this.Controls.Add(this.btnStartAsync);
            this.Name = "AsyncDemoForm";
            this.Text = "AsyncDemoForm";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnStartAsync;
        private System.Windows.Forms.Label lblExecuting;
        private System.Windows.Forms.Label lblComplete;
    }
}