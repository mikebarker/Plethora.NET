using System;
using System.Windows.Forms;

using Plethora.Drawing.Sample.Properties;

namespace Plethora.Drawing.Sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();
        }

        private void radioColour_CheckedChanged(object sender, EventArgs e)
        {
            this.pictureBox1.Image = (this.radGreyscale.Checked)
                ? Resources.Pinata.ToGrayscale()
                : Resources.Pinata;
        }
    }
}
