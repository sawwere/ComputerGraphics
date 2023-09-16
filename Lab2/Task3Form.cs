using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FastBitmap;

namespace Lab2
{
    public partial class Task3Form : Form
    {
        private Bitmap buffer;
        public Task3Form()
        {
            InitializeComponent();
        }

        private (double, double, double) ConvertHSVtoRGB(double h, double s, double v)
        {
            s *= 100;
            v *= 100;
            int hi = ((int)Math.Floor(h / 60)) % 6;
            double vmin = ((100 - s) * v / 100);
            double a = (v - vmin) * ((int)h % 60) / 60;
            double vinc = vmin + a;
            double vdec = v - a;

            v *= 2.55;
            vinc *= 2.55;
            vdec *= 2.55;
            vmin *= 2.55;


            switch (hi)
            {
                case 0:
                    return (v, vinc, vmin);
                case 1:
                    return (vdec, v, vmin);
                case 2:
                    return (vmin, v, vinc);
                case 3:
                    return (vmin, vdec, v);
                case 4:
                    return (vinc, vmin, v);
                case 5:
                    return (v, vmin, vdec);
                default:
                    return (0, 0, 0);
            }
        }

        private (double, double, double) ConvertRGBtoHSV(double r, double g, double b)
        {
            double MAX = (new double[] { r, g, b}).Max();
            double MIN = (new double[] { r, g, b }).Min();
            double h, s, v;
            if (Math.Abs(MAX - MIN) < 0.0001)
            {
                h = 0;
            }
            else if (Math.Abs(MAX - r) < 0.0001 && g >= b)
            {
                h = 60 * ((g - b) / (MAX - MIN));
            }
            else if (Math.Abs(MAX - r) < 0.0001 && g < b)
            {
                h = 60 * ((g - b) / (MAX - MIN)) + 360;
            }
            else if (Math.Abs(MAX - g) < 0.0001)
            {
                h = 60 * ((b - r) / (MAX - MIN)) + 120;
            }
            else
            {
                h = 60 * ((r - g) / (MAX - MIN)) + 240;
            }

            if (Math.Abs(MAX - 0) < 0.0001)
            {
                s = 0;
            }
            else
            {
                s = 1 - (MIN / MAX);
            }

            v = MAX;
            return  (h, s, v);
        }

        void Draw()
        {
            double deltaH = trackBar1.Value;
            double deltaS = trackBar2.Value / 100.0;
            double deltaV = trackBar3.Value / 100.0;
            Bitmap newImage = (Bitmap)buffer.Clone();

            using (var fastBitmap = new FastBitmap.FastBitmap(newImage))
            {
                for (var x = 0; x < fastBitmap.Width; x++)
                    for (var y = 0; y < fastBitmap.Height; y++)
                    {
                        var color = fastBitmap[x, y];
                        var change = ConvertRGBtoHSV(color.R / 255.0, color.G / 255.0, color.B / 255.0);

                        change.Item1 += deltaH;
                        change.Item1 = change.Item1 % 360;
                        change.Item2 += deltaS;
                        change.Item2 = Math.Min(change.Item2, 1);
                        change.Item3 += deltaV;
                        change.Item3 = Math.Min(change.Item3, 1);

                        var rgb = ConvertHSVtoRGB(change.Item1, change.Item2, change.Item3);
                        fastBitmap[x, y] = Color.FromArgb((int)(rgb.Item1), (int)(rgb.Item2), (int)(rgb.Item3));
                    }
                
            }
            pictureBox2.Image = newImage;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = (this.Owner as Form1)._image;
            buffer = new Bitmap(pictureBox1.Image);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Draw();
        }
    }
}
