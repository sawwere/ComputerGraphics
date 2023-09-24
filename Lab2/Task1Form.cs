using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab2
{
    public partial class Task1Form : Form
    {
        private Bitmap buffer;

        public Task1Form()
        {
            InitializeComponent();
        }

        void Draw()
        {
            Bitmap newImage = (Bitmap)buffer.Clone();
            Bitmap newImage2 = (Bitmap)buffer.Clone();
            Bitmap newImage3 = (Bitmap)buffer.Clone();

            //Pixel[] points = new Pixel[newImage3.Height * newImage3.Width];
            int[] mas = new int[256];
            Array.Clear(mas, 0, 256);
            double[] points = new double[newImage3.Height * newImage3.Width];

            using (var fastBitmap = new FastBitmap.FastBitmap(newImage))
            {
                for (var x = 0; x < fastBitmap.Width; x++)
                {
                    for (var y = 0; y < fastBitmap.Height; y++)
                    {
                        var color = fastBitmap[x, y];
                        var Y = 0.299 * color.R + 0.587 * color.G + 0.114 * color.B;
                        fastBitmap[x, y] = Color.FromArgb((int)(Y), (int)(Y), (int)(Y));
                        mas[(int)Y]++;

                    }
                }
            }
            pictureBox2.Image = newImage;
            
            for (var i = 0; i < 256; i++) {
                chart1.Series["Grey"].Points.AddXY(i, mas[i]);
            }

            Array.Clear(mas, 0, 256);
            using (var fastBitmap = new FastBitmap.FastBitmap(newImage2))
            {
                for (var x = 0; x < fastBitmap.Width; x++)
                {
                    for (var y = 0; y < fastBitmap.Height; y++)
                    {
                        var color = fastBitmap[x, y];
                        var Y = 0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B;
                        fastBitmap[x, y] = Color.FromArgb((int)(Y), (int)(Y), (int)(Y));
                        mas[(int)Y]++;
                    }
                }
            }
            pictureBox3.Image = newImage2;
            for (var i = 0; i < 256; i++)
            {
                chart2.Series["Grey"].Points.AddXY(i, mas[i]);
            }
            Array.Clear(mas, 0, 256);

            var min = Double.MaxValue;
            var max = Double.MinValue;
            var count = 0;

            using (var fastBitmap = new FastBitmap.FastBitmap(newImage3))
            {
                for (var x = 0; x < fastBitmap.Width; x++)
                {
                    for (var y = 0; y < fastBitmap.Height; y++)
                    {

                        var color = fastBitmap[x, y];
                        var Y = ( 0.2126 * color.R + 0.7152 * color.G + 0.0722 * color.B) - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B);
                        if (Y<min)
                        {
                            min = Y;
                        }
                        if (Y > max)
                        {
                            max = Y;
                        }
                        points[count] = Y;
                        count++;
                        
                    }
                }
            }
          

            count = 0;
            using (var fastBitmap = new FastBitmap.FastBitmap(newImage3))
            {
                for (var x = 0; x < fastBitmap.Width; x++)
                {
                    for (var y = 0; y < fastBitmap.Height; y++)
                    {
                        var r = (points[count] - min) * 255 / (max - min);
                        count++;
                        fastBitmap[x, y] = Color.FromArgb((int)(r), (int)(r), (int)(r));
                        mas[(int)r]++;
                    }
                }
            }


            pictureBox4.Image = newImage3;
            for (var i = 0; i < 256; i++)
            {
                chart3.Series["Grey"].Points.AddXY(i, mas[i]);
            }
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

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

        private void chart1_Click(object sender, EventArgs e)
        {
            
        }
    }
}
