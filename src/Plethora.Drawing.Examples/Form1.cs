using System;
using System.Windows.Forms;
using Plethora.Drawing.Examples.Properties;

namespace Plethora.Drawing.Examples
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void radioColour_CheckedChanged(object sender, EventArgs e)
        {
            pictureBox1.Image = (radGreyscale.Checked)
                ? Resources.Pinata.ToGrayscale()
                : Resources.Pinata;
        }
    }
}
