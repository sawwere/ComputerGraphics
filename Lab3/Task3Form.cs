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


namespace Lab3
{
    public partial class Task3Form : Form
    {
        class MyPoint
        {
            //private Color c;
            //private int x, y;

            public MyPoint(int x, int y, Color c)
            {
                this.X = x;
                this.Y = y;
                this.C = c;
            }

            public int X { get; set; }
            public int Y { get; set; }
            public Color C { get; set; }
        }

        private MyPoint[] points;
        private Graphics g;
        private Bitmap bitmap;
        private int currentPointer;

        public Task3Form()
        {
            InitializeComponent();
            points = new MyPoint[3] { new MyPoint(0, 0, Color.Red), 
                                      new MyPoint(0, 0, Color.Green), 
                                      new MyPoint(0, 0, Color.Black) };
            //g = this.CreateGraphics();
            button0.BackColor = points[0].C;
            button1.BackColor = points[1].C;
            button2.BackColor = points[2].C;
            g = Graphics.FromImage(pictureBox1.Image);
            g.Clear(Color.White);
            bitmap = new Bitmap(pictureBox1.Image);
            currentPointer = 0;
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            (sender as Button).BackColor = colorDialog1.Color;
            points[int.Parse((sender as Button).Tag.ToString())].C = colorDialog1.Color;
        }

        private void DrawPoint(int X, int Y, Color c, int width = 2)
        {
            using (var fastBitmap = new FastBitmap.FastBitmap(bitmap))
                for (int x = -width; x < width; x++)
                {
                    for (int y = -width; y < width; y++)
                        if (X + x > 0 && X + x < this.Width && Y + y > 0 && Y + y < Height)
                            fastBitmap.SetPixel(new Point(X + x, Y + y), c);
                }
        }

        private void panel1_MouseClick(object sender, MouseEventArgs e)
        {
            DrawPoint(points[currentPointer].X, points[currentPointer].Y, Color.White);
            points[currentPointer].X = e.X;
            points[currentPointer].Y = e.Y;
            var label = this.Controls.Find($"label{currentPointer}", false).First();
            label.Text = $"( {e.X }; {e.Y} )";
            DrawPoint(e.X, e.Y, points[currentPointer].C);
            currentPointer++;
            currentPointer %= 3;
            pictureBox1.Image = bitmap;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(Lab3.Properties.Resources.grid);
            pictureBox1.Image = bitmap;
        }
    }
}
