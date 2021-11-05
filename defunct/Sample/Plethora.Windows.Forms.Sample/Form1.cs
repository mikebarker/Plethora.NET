using System.Drawing;
using System.Windows.Forms;

namespace Plethora.Windows.Forms.Sample
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            this.InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Point p = new Point(this.button1.Left, this.button1.Bottom);
            p = this.PointToScreen(p);

            Size s = new Size(this.button1.Width, 20);

            Popup.TextBox(p, s, 
                text => this.button1.Text = text);
        }

    }
}

