using System.Drawing;
using System.Windows.Forms;

namespace Plethora.Windows.Forms.Examples
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, System.EventArgs e)
        {
            Point p = new Point(button1.Left, button1.Bottom);
            p = this.PointToScreen(p);

            Size s = new Size(button1.Width, 20);

            Popup.TextBox(p, s, 
                text => button1.Text = text);
        }

    }
}

