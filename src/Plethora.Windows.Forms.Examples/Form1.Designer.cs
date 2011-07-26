using System;

namespace Plethora.Windows.Forms.Examples
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
            this.components = new System.ComponentModel.Container();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBoxEx1 = new Plethora.Windows.Forms.TextBoxEx();
            this.integerTextBox1 = new Plethora.Windows.Forms.IntegerTextBox();
            this.numericTextBoxStyliserComponent1 = new Plethora.Windows.Forms.Styles.NumericTextBoxStyliserComponent(this.components);
            this.dateTimeTextBox1 = new Plethora.Windows.Forms.DateTimeTextBox();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(61, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "TextBoxEx:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 119);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(95, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "DateTimeTextBox:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 184);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Popup:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 56);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(85, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Integer TextBox:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(113, 179);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Click Me";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(192, 235);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(80, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "checkBox1";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // textBoxEx1
            // 
            this.textBoxEx1.Location = new System.Drawing.Point(113, 12);
            this.textBoxEx1.Name = "textBoxEx1";
            this.textBoxEx1.Size = new System.Drawing.Size(144, 20);
            this.textBoxEx1.TabIndex = 0;
            this.textBoxEx1.Text = "Triple click to select all text.";
            // 
            // integerTextBox1
            // 
            this.integerTextBox1.Location = new System.Drawing.Point(113, 53);
            this.integerTextBox1.MaxValue = ((long)(9223372036854775807));
            this.integerTextBox1.MinValue = ((long)(-9223372036854775808));
            this.integerTextBox1.Name = "integerTextBox1";
            this.integerTextBox1.Size = new System.Drawing.Size(144, 20);
            this.integerTextBox1.Styliser = Plethora.Windows.Forms.Styles.NumericTextBoxStyliser.Default;
            this.integerTextBox1.TabIndex = 1;
            this.integerTextBox1.Text = "0";
            this.integerTextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // numericTextBoxStyliserComponent1
            // 
            this.numericTextBoxStyliserComponent1.NegativeStyle = null;
            this.numericTextBoxStyliserComponent1.PositiveStyle = null;
            this.numericTextBoxStyliserComponent1.ZeroStyle = null;
            // 
            // dateTimeTextBox1
            // 
            this.dateTimeTextBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.dateTimeTextBox1.Location = new System.Drawing.Point(113, 116);
            this.dateTimeTextBox1.MaxValue = new System.DateTime(9999, 12, 31, 23, 59, 59, 999);
            this.dateTimeTextBox1.MinValue = new System.DateTime(((long)(0)));
            this.dateTimeTextBox1.Name = "dateTimeTextBox1";
            this.dateTimeTextBox1.Size = new System.Drawing.Size(159, 20);
            this.dateTimeTextBox1.TabIndex = 2;
            this.dateTimeTextBox1.Text = "6/7/2011";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 264);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.textBoxEx1);
            this.Controls.Add(this.integerTextBox1);
            this.Controls.Add(this.dateTimeTextBox1);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private TextBoxEx textBoxEx1;
        private DateTimeTextBox dateTimeTextBox1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label4;
        private IntegerTextBox integerTextBox1;
        private System.Windows.Forms.CheckBox checkBox1;
        private Plethora.Windows.Forms.Styles.NumericTextBoxStyliserComponent numericTextBoxStyliserComponent1;


    }
}

