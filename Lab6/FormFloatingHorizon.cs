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
using Tools.Primitives;


namespace Lab6
{
    public partial class FormFloatingHorizon : Form
    {
        Mesh figure = null;
        Graphic gra = null;
        Graphics g;
        public double phi = 60, psi = 100;
        public FormFloatingHorizon() {
            InitializeComponent();
            pictureBox1.Image = new Bitmap(pictureBox1.Width, pictureBox1.Height);
            g = Graphics.FromImage(pictureBox1.Image);
            g.TranslateTransform(pictureBox1.ClientSize.Width / 2, pictureBox1.ClientSize.Height / 2);
            g.ScaleTransform(1, -1);
        }




        private void buttonBuild_Click(object sender, EventArgs e)
        {
            gra = new Graphic(comboBox1.SelectedIndex);

            figure = gra;

            g.Clear(Color.White);
            //Graph.isGraph = true;
            gra.picture = pictureBox1;
            gra.DrawGraphic();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            g.Clear(Color.White);
        }

        private void FormFloatingHorizon_KeyPress(object sender, KeyPressEventArgs e)
        {
                if (e.KeyChar ==' ')
                    psi -= 10;
                else if (e.KeyChar == 'd')
                    psi += 10;
                else if (e.KeyChar == 'w')
                    phi -= 10;
                else if (e.KeyChar == 's')
                    phi += 10;
            gra.DrawGraphic();

        }

        private void FormFloatingHorizon_KeyDown(object sender, KeyEventArgs e)
        {
            //if (e.KeyCode == Keys.Space)
            //    psi -= 10;
            //else if (e.KeyCode == Keys.D)
            //    psi += 10;
            //else if (e.KeyCode == Keys.W)
            //    phi -= 10;
            //else if (e.KeyCode == Keys.S)
            //    phi += 10;
            //gra.DrawGraphic();
        }
    }
    class Graphic : Mesh
    {
        public Func<double, double, double> F;
        public int X0 { get; }
        public int X1 { get; }
        public int Y0 { get; }
        public int Y1 { get; }
        public int CountOfSplits { get; }

        public PictureBox picture;

        public double phi = 60, psi = 100;

        public List<Triangle3D> Polygons;

        readonly Func<double, double, double> func = (x, y) => Math.Cos(Math.Sqrt(x * x + y * y));

        //границы горизонта
        List<double> upFloatingHorizon, downFloatingHorizon;

        public Graphic(int func)
        {
            switch (func)
            {
                case 0:
                    F = (x, y) => x + y;
                    break;
                case 1:
                    F = (x, y) => (float)Math.Cos(x * x + y * y);
                    break;
                case 2:
                    F = (x, y) => (float)Math.Sin(x) + (float)Math.Cos(y);
                    break;
                case 3:
                    F = (x, y) => (float)Math.Sin(x);
                    break;
                case 4:
                    F = (x, y) => Math.Cos(Math.Sqrt(x * x + y * y));
                    break;
                case 5:
                    F = (x, y) => Math.Sin(Math.Sqrt(x * x + y * y));
                    break;
                default:
                    F = (x, y) => x + y;
                    break;
            }
        }

        public void DrawGraphic()
        {
            //создаем и заполняем границы горизонта
            upFloatingHorizon = new List<double>(picture.Width);
            downFloatingHorizon = new List<double>(picture.Width);
            for (int i = 0; i < picture.Width; i++)
            {
                upFloatingHorizon.Add(0);
                downFloatingHorizon.Add(1000);
            }

            Bitmap res = new Bitmap(picture.Width, picture.Height);

            for (double x = -5; x <= 5.001; x += 0.2)
            {
                List<Point> currentCurve = new List<Point>();
                for (double y = -5; y <= 5.001; y += 0.2)
                {
                    double z = F(x, y);

                    //отображение координат на данной проекции
                    double _phi = phi * Math.PI / 180, _psi = psi * Math.PI / 180;
                    double fx = x * Math.Cos(_psi) - (-Math.Sin(_phi) * y + Math.Cos(_phi) * z) * Math.Sin(_psi);
                    double fy = y * Math.Cos(_psi) + z * Math.Sin(_phi);
                    double k = 50d;

                    //отображение на pictureBox
                    currentCurve.Add(new Point((int)Math.Round(picture.Width / 2 + fx * k), (int)Math.Round(picture.Height / 2 + fy * k)));
                }
                DrawCurve(res, currentCurve, upFloatingHorizon, downFloatingHorizon);
            }
            if (picture.Image != null) picture.Image.Dispose();
            picture.Image = res;
        }

        public void DrawCurve(Bitmap res, List<Point> curve, List<double> upFloatingHorizon, List<double> downFloatingHorizon)
        {
            for (int i = 1; i < curve.Count; i++)
            {
                Point p1 = curve[i - 1];
                Point p2 = curve[i];

                int x1 = p1.X, x2 = p2.X, y1 = p1.Y, y2 = p2.Y;
                bool needChange = Math.Abs(y2 - y1) > Math.Abs(x2 - x1);

                if (needChange)
                {
                    (x1, y1) = (y1,x1);
                    (x2, y2) = (y2, x2);
                }
                if (x1 > x2)
                {
                    (x1, x2) = (x2, x1);
                    (y1, y2) = (y2, y1);
                }

                double df = (y2 * 1.0 - y1) / (x2 * 1.0 - x1); //находим градиент
                double y = y1 + df;

                for (int x = x1 + 1; x < x2; x++)
                {
                    int xx1 = needChange ? (int)Math.Round(y) : x;
                    int xx2 = needChange ? (int)Math.Round(y) : x;
                    int yy1 = needChange ? x : (int)Math.Round(y);
                    int yy2 = needChange ? x : (int)Math.Round(y + 1);

                    if (xx1 < 0 || xx2 < 0 || yy1 < 0 || yy2 < 0) continue;
                    if (xx1 >= res.Width || xx2 >= res.Width || yy1 >= res.Height || yy2 >= res.Height) continue;

                    if ((yy1 >= upFloatingHorizon[xx1] && yy2 >= upFloatingHorizon[xx2]))
                    {
                        res.SetPixel(xx2, yy2, Color.Black);
                        upFloatingHorizon[xx1] = yy1;
                        upFloatingHorizon[xx2] = yy2;
                    }
                    else if (yy1 <= downFloatingHorizon[xx1] && yy2 <= downFloatingHorizon[xx2])
                    {
                        res.SetPixel(xx1, yy1, Color.Black);
                        res.SetPixel(xx2, yy2, Color.Black);
                        downFloatingHorizon[xx1] = yy1;
                        downFloatingHorizon[xx2] = yy2;

                    }
                    y += df;
                }
            }
        }   
    }

    public sealed class ReverseFloatComparer : IComparer<float>
    {
        public int Compare(float x, float y)
        {
            return y.CompareTo(x);
        }
    }
    class Graph : Mesh
    {
        public Func<double, double, double> F;
        public int X0 { get; }
        public int X1 { get; }
        public int Y0 { get; }
        public int Y1 { get; }
        public int CountOfSplits { get; }
        public List<Triangle3D> Polygons;
        public Graph(int x0, int x1, int y0, int y1, int count, int func)
        {
            X0 = x0;
            X1 = x1;
            Y0 = y0;
            Y1 = y1;
            CountOfSplits = count;
            Polygons = new List<Triangle3D>();

            float dx = (Math.Abs(x0) + Math.Abs(x1)) / (float)count;
            float dy = (Math.Abs(y0) + Math.Abs(y1)) / (float)count;

            List<Point3D> points0 = new List<Point3D>();
            List<Point3D> points = new List<Point3D>();

            switch (func)
            {
                case 0:
                    F = (x, y) => x + y;
                    break;
                case 1:
                    F = (x, y) => (float)Math.Cos(x * x + y * y);
                    break;
                case 2:
                    F = (x, y) => (float)Math.Sin(x) * 10f + (float)Math.Cos(y) * 10f;
                    break;
                case 3:
                    F = (x, y) => (float)Math.Sin(x) * 5f;
                    break;
                case 4:
                    F = (x, y) => x + (y * y);
                    break;
                default:
                    F = (x, y) => x + y;
                    break;
            }

            for (float x = x0; x < x1; x += dx)
            {
                for (float y = y0; y < y1; y += dy)
                {
                    var z = F(x, y);
                    points.Add(new Point3D(x, y, (float)z));
                }

                if (points0.Count != 0)
                    for (int i = 1; i < points0.Count; ++i)
                    {
                        Polygons.Add(new Triangle3D(

                            new Point3D(points0[i - 1].X, points0[i - 1].Y, points0[i - 1].Z),
                            new Point3D(points[i - 1].X, points[i - 1].Y, points[i - 1].Z),
                            new Point3D(points[i].X, points[i].Y, points[i].Z)
                         ));
                        Polygons.Add(new Triangle3D(
                           new Point3D(points[i - 1].X, points[i - 1].Y, points[i - 1].Z),
                           new Point3D(points[i].X, points[i].Y, points[i].Z),
                           new Point3D(points0[i].X, points0[i].Y, points0[i].Z)));
                    }
                points0.Clear();
                points0 = points;
                points = new List<Point3D>();
            }
        }
    }

}
