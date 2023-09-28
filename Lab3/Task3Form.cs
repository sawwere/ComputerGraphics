using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Tools;
using Tools.FastBitmap;


namespace Lab3
{
    public partial class Task3Form : Form
    {
        private Point2D[] points;
        private Graphics g;
        private Bitmap bitmap;
        private int currentPointer;

        public Task3Form()
        {
            InitializeComponent();
            points = new Point2D[3] { new Point2D(294, 83, Color.Red),
                                      new Point2D(143, 252, Color.Green),
                                      new Point2D(432, 252, Color.Black) };
            g = this.CreateGraphics();
            button0.BackColor = points[0].Color;
            button1.BackColor = points[1].Color;
            button2.BackColor = points[2].Color;
            g.Clear(Color.White);
            bitmap = new Bitmap(pictureBox1.Image);
            currentPointer = 0;
            for (int i = 0; i < 3; i++)
            {
                this.Controls.Find($"label{i}", false).First().Text = $"( {points[i].X }; {points[i].Y} )";
            }
        }

        int Interpolate(int x0, int y0, int x1, int y1, int i)
        {
            if (x0 == x1)
                return y0;
            return y0 + ((y1 - y0) * (i - x0)) / (x1 - x0);
        }

        private void buttonColor_Click(object sender, EventArgs e)
        {
            if (colorDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            (sender as Button).BackColor = colorDialog1.Color;
            points[int.Parse((sender as Button).Tag.ToString())].Color = colorDialog1.Color;
        }

        private void DrawPoint(int X, int Y, Color c, int width = 2)
        {
            using (var fastBitmap = new FastBitmap(bitmap))
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
            DrawPoint(e.X, e.Y, points[currentPointer].Color);
            currentPointer++;
            currentPointer %= 3;
            pictureBox1.Image = bitmap;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(Lab3.Properties.Resources.grid);
            pictureBox1.Image = bitmap;
            currentPointer = 0;
        }

        private void DrawGradientLines(int y, Point2D middle, Point2D left, Point2D right)
        {
            using (var fastBitmap = new FastBitmap(bitmap))
            {
                var leftBound = Interpolate(middle.Y, middle.X, left.Y, left.X, y);
                var rightBound = Interpolate(middle.Y, middle.X, right.Y, right.X, y);

                var colorStartR = Interpolate(middle.Y, middle.Color.R,
                    left.Y, left.Color.R, y);
                var colorStartG = Interpolate(middle.Y, middle.Color.G,
                    left.Y, left.Color.G, y);
                var colorStartB = Interpolate(middle.Y, middle.Color.B,
                    left.Y, left.Color.B, y);

                var colorEndR = Interpolate(middle.Y, middle.Color.R,
                    right.Y, right.Color.R, y);
                var colorEndG = Interpolate(middle.Y, middle.Color.G,
                    right.Y, right.Color.G, y);
                var colorEndB = Interpolate(middle.Y, middle.Color.B,
                    right.Y, right.Color.B, y);
                for (int x = leftBound; x <= rightBound; x++)
                {
                    var colorR = Interpolate(leftBound, colorStartR, rightBound, colorEndR, x);
                    var colorG = Interpolate(leftBound, colorStartG, rightBound, colorEndG, x);
                    var colorB = Interpolate(leftBound, colorStartB, rightBound, colorEndB, x);

                    fastBitmap[x, y] = Color.FromArgb(colorR, colorG, colorB);

                }
            }
        }

        private void buttonDraw_Click(object sender, EventArgs e)
        {
            var sortedPoints = points.OrderBy(x => x.Y).ToList();

            Point2D left = sortedPoints[1];
            Point2D right = sortedPoints[2];
            if ((sortedPoints[1].X > sortedPoints[2].X) && (sortedPoints[1].X > sortedPoints[0].X))
            {
                left = sortedPoints[2];
                right = sortedPoints[1];
            }
            for (int y = sortedPoints[0].Y; y < sortedPoints[1].Y; y++)
            {
                DrawGradientLines(y, sortedPoints[0], left, right);
            }

            left = sortedPoints[1];
            right = sortedPoints[0];
            if (sortedPoints[2].X < sortedPoints[1].X && sortedPoints[0].X < sortedPoints[1].X)
            {
                left = sortedPoints[0];
                right = sortedPoints[1];
            }
            for (int y = sortedPoints[2].Y; y >= sortedPoints[1].Y; y--)
            {
                DrawGradientLines(y, sortedPoints[2], left, right);
            }

            pictureBox1.Image = bitmap;
        }
    }
}
