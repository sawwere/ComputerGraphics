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
    public partial class Task2Form : Form
    {
        private Bitmap buffer;
        public Task2Form()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = (this.Owner as Form1)._image;
            buffer = new Bitmap(pictureBox1.Image);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Bitmap newImage = (Bitmap)buffer.Clone();
            using (var fastBitmap = new FastBitmap.FastBitmap(newImage))
            {
                for (var x = 0; x < fastBitmap.Width; x++)
                    for (var y = 0; y < fastBitmap.Height; y++)
                    {
                        fastBitmap[x, y] = Color.FromArgb(fastBitmap[x,y].R,0,0);
                    }

            }
            pictureBox2.Image = newImage;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Bitmap newImage = (Bitmap)buffer.Clone();
            using (var fastBitmap = new FastBitmap.FastBitmap(newImage))
            {
                for (var x = 0; x < fastBitmap.Width; x++)
                    for (var y = 0; y < fastBitmap.Height; y++)
                    {
                        fastBitmap[x, y] = Color.FromArgb(0, fastBitmap[x, y].G, 0);
                    }

            }
            pictureBox2.Image = newImage;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Bitmap newImage = (Bitmap)buffer.Clone();
            using (var fastBitmap = new FastBitmap.FastBitmap(newImage))
            {
                for (var x = 0; x < fastBitmap.Width; x++)
                    for (var y = 0; y < fastBitmap.Height; y++)
                    {
                        fastBitmap[x, y] = Color.FromArgb(0, 0, fastBitmap[x, y].B);
                    }

            }
            pictureBox2.Image = newImage;
        }
    }
}
