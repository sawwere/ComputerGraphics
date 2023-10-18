using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    public partial class Task2Form : Form
    {
        Graphics g;
        Pen p;
        bool f;
        private Color brown = Color.Red;
        public Task2Form()
        {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            numericUpDown1.Maximum = pictureBox1.Height;
            numericUpDown2.Maximum = pictureBox1.Height;
            pictureBox1.BackColor = Color.White;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            button1.Enabled = false;
            button2.Enabled = false;

            button2_Click(sender, e);
            int c = (int)numericUpDown3.Value;
            p = new Pen(Color.Blue, 2);
            g.DrawLine(p, 0, (int)numericUpDown1.Value, pictureBox1.Width, (int)numericUpDown2.Value);
            pictureBox1.Refresh();
            midpoint_displ((int)numericUpDown1.Value, 0, (int)numericUpDown2.Value, pictureBox1.Width, c);

            button1.Enabled = true;
            button2.Enabled = true;
        }

        private int Interpolate(int x0, int y0, int x1, int y1, int i)
        {
            if (x0 == x1)
                return y0;
            return y0 + ((y1 - y0) * (i - x0)) / (x1 - x0);
        }

        private void midpoint_displ(int h1, int x1, int h2, int x2, int c)
        {
            if (c > 0)
            {
                --c;
                int dis_x = x2 - x1;
                int dis_h = h2 - h1;
                double l = Math.Sqrt(dis_x * dis_x + dis_h * dis_h);
                Random r = new Random();
                double h = (h1 + h2) / 2.0;
                double dis_hh = h1 - h;
                double l2 = l / 2.0;
                int x = x1 + (int)Math.Round(Math.Sqrt(l2 * l2 - dis_hh * dis_hh));
                int rand = r.Next((int)((-1) * numericUpDown4.Value * (int)Math.Round(l)), (int)(numericUpDown4.Value * (int)Math.Round(l)));
                h += rand;

                if (h < 0)
                    h = 10;
                if (h > pictureBox1.Height)
                    h = pictureBox1.Height - 10;

                var colorR = Interpolate(0, brown.R, (int)numericUpDown3.Value, 0, c);
                var colorG = Interpolate(0, brown.G, (int)numericUpDown3.Value, 255, c);
                var colorB = Interpolate(0, brown.B, (int)numericUpDown3.Value, 0, c);
                var curPen = new Pen(Color.FromArgb(255, colorR, colorG, colorB));

                g.DrawLine(curPen, x1, h1, x, (int)Math.Ceiling(h));
                g.DrawLine(curPen, x, (int)Math.Ceiling(h), x2, h2);
                pictureBox1.Refresh();
                midpoint_displ(h1, x1, (int)Math.Ceiling(h), x, c);
                midpoint_displ((int)Math.Ceiling(h), x, h2, x2, c);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image.Dispose();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
        }

    }
}
