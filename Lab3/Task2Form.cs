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
    public partial class Task2Form : Form
    {
        List<Point> points = new List<Point>();
        private Graphics g;
        private Bitmap bitmap;
        private int currentPointer;
        public Task2Form()
        {
            InitializeComponent();
            bitmap = new Bitmap(pictureBox1.Image);
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

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Point point = pictureBox1.PointToClient(Cursor.Position);
            using (Graphics g = pictureBox1.CreateGraphics())
            {
                g.FillRectangle(Brushes.Black, point.X, point.Y, 3, 3);
            }
            points.Add(point);
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            bitmap = new Bitmap(Lab3.Properties.Resources.grid);
            pictureBox1.Image = bitmap;
            currentPointer = 0;
            points.Clear();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            int x0 = points[points.Count - 2].X;
            int y0 = points[points.Count - 2].Y;
            int x1 = points[points.Count-1].X; 
            int y1 = points[points.Count - 1].Y;
            Bresenham(x0,y0,x1,y1);
            pictureBox1.Image = bitmap;
        }
        void Bresenham(int x1, int y1, int x2, int y2)
        {

            if (x1 > x2)
            {
                int t = y1;
                y1 = y2;
                y2 = t;
                t = x1;
                x1 = x2;
                x2 = t;
            }
            int dx = x2 - x1;
            int dy = y2 - y1;
            int xi = x1;
            int yi = y1;
            int step = 1;
            int di = 2 * dy - dx;
            //1 и 4 четверти
            if (dx == 0 || Math.Abs(dy / (double)dx) > 1)
            {
                //4 четверть
                if (dy / (double)dx < 0)
                {
                    xi = x2;
                    step = -1;
                    dy = -dy;
                    int t = y1;
                    y1 = y2;
                    y2 = t;
                }
                for (yi = y1; yi <= y2; yi++)
                {
                    bitmap.SetPixel(xi, yi, Color.Black);
                    if (di >= 0)
                    {
                        xi += step;
                        di += 2 * (dx - dy);
                    }
                    else
                    {
                        di += 2 * dx;
                    }
                }
            }
            else
            {
                if (dy / (double)dx < 0)
                {
                    step = -1;
                    dy = -dy;
                }
                for (xi = x1; xi <= x2; xi++)
                {
                    bitmap.SetPixel(xi, yi, Color.Black);
                    if (di >= 0)
                    {
                        yi += step;
                        di += 2 * (dy - dx);
                    }
                    else
                    {
                        di += 2 * dy;
                    }
                }
            }
        }
        void Wu(int x1, int y1, int x2, int y2)
        {
            if (x1 > x2)
            {
                int t = y1;
                y1 = y2;
                y2 = t;
                t = x1;
                x1 = x2;
                x2 = t;
            }
            int dx = x2 - x1;
            int dy = y2 - y1;
            double gradient;
            if (dx == 0)
            {
                gradient = 1;
            }
            else
            {
                gradient = dy / (double)dx;
            }
            int step = 1;
            double xi = x1;
            double yi = y1;

            //1 и 4 четверти
            if (Math.Abs(gradient) > 1)
            {
                gradient = 1 / gradient;
                //4 четверть
                if (gradient < 0)
                {
                    xi = x2;
                    step = -1;
                    int t = y1;
                    y1 = y2;
                    y2 = t;
                }

                for (yi = y1; yi <= y2; yi += 1)
                {
                    int help;
                    if (gradient < 0)
                    {
                        help = (int)(255 * (xi - (int)xi));
                    }
                    else
                    {
                        help = 255 - (int)(255 * (xi - (int)xi));
                    }
                    bitmap.SetPixel((int)xi, (int)yi, Color.FromArgb(255 - help, 255 - help, 255 - help));
                    bitmap.SetPixel((int)xi + step, (int)yi, Color.FromArgb(help, help, help));
                    xi += gradient;
                }
            }
            else
            {
                //3 четверть
                if (gradient < 0)
                {
                    step = -1;
                }
                for (xi = x1; xi <= x2; xi += 1)
                {
                    int help;
                    if (gradient < 0)
                    {
                        help = (int)(255 * (yi - (int)yi));
                    }
                    else
                    {
                        help = 255 - (int)(255 * (yi - (int)yi));
                    }
                    bitmap.SetPixel((int)xi, (int)yi, Color.FromArgb(255 - help, 255 - help, 255 - help));
                    bitmap.SetPixel((int)xi, (int)yi + step, Color.FromArgb(help, help, help));
                    yi += gradient;
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int x0 = points[points.Count - 2].X;
            int y0 = points[points.Count - 2].Y;
            int x1 = points[points.Count - 1].X;
            int y1 = points[points.Count - 1].Y;
            Wu(x0, y0, x1, y1);
            pictureBox1.Image = bitmap;
        }
    }
}
