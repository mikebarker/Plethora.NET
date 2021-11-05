namespace Plethora.Drawing.Sample
{
    partial class Form1
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
            if (disposing && (this.components != null))
            {
                this.components.Dispose();
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
            this.grpChooseColour = new System.Windows.Forms.GroupBox();
            this.radFullColour = new System.Windows.Forms.RadioButton();
            this.radGreyscale = new System.Windows.Forms.RadioButton();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.grpChooseColour.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // grpChooseColour
            // 
            this.grpChooseColour.Controls.Add(this.radGreyscale);
            this.grpChooseColour.Controls.Add(this.radFullColour);
            this.grpChooseColour.Location = new System.Drawing.Point(12, 12);
            this.grpChooseColour.Name = "grpChooseColour";
            this.grpChooseColour.Size = new System.Drawing.Size(94, 65);
            this.grpChooseColour.TabIndex = 0;
            this.grpChooseColour.TabStop = false;
            this.grpChooseColour.Text = "Choose Colour";
            // 
            // radFullColour
            // 
            this.radFullColour.AutoSize = true;
            this.radFullColour.Checked = true;
            this.radFullColour.Location = new System.Drawing.Point(6, 19);
            this.radFullColour.Name = "radFullColour";
            this.radFullColour.Size = new System.Drawing.Size(74, 17);
            this.radFullColour.TabIndex = 0;
            this.radFullColour.TabStop = true;
            this.radFullColour.Text = "Full Colour";
            this.radFullColour.UseVisualStyleBackColor = true;
            this.radFullColour.CheckedChanged += new System.EventHandler(this.radioColour_CheckedChanged);
            // 
            // radGreyscale
            // 
            this.radGreyscale.AutoSize = true;
            this.radGreyscale.Location = new System.Drawing.Point(6, 42);
            this.radGreyscale.Name = "radGreyscale";
            this.radGreyscale.Size = new System.Drawing.Size(72, 17);
            this.radGreyscale.TabIndex = 0;
            this.radGreyscale.Text = "Greyscale";
            this.radGreyscale.UseVisualStyleBackColor = true;
            this.radGreyscale.CheckedChanged += new System.EventHandler(this.radioColour_CheckedChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Plethora.Drawing.Sample.Properties.Resources.Pinata;
            this.pictureBox1.Location = new System.Drawing.Point(112, 21);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(375, 402);
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(498, 433);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.grpChooseColour);
            this.Name = "Form1";
            this.Text = "Form1";
            this.grpChooseColour.ResumeLayout(false);
            this.grpChooseColour.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox grpChooseColour;
        private System.Windows.Forms.RadioButton radFullColour;
        private System.Windows.Forms.RadioButton radGreyscale;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

