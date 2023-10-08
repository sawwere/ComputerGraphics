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

namespace IndTask1
{
    public partial class Form1 : Form
    {
        private Graphics g;
        private Bitmap bitmap;
        private List<Point2D> innerPoints;
        Polygon polygon;
        public Form1()
        {
            InitializeComponent();
            g = this.CreateGraphics();
            g.Clear(Color.White);
            bitmap = new Bitmap(pictureBox1.Image);
            g = Graphics.FromImage(bitmap);
            InitPrimitives();
        }

        private void InitPrimitives()
        {
            polygon = new Polygon();
            innerPoints = new List<Point2D>();
        }

        private void Clear()
        {
            g.Clear(Color.White);
            pictureBox1.Image = bitmap;
        }

        private void Draw()
        {
            polygon.Draw(g);
            foreach (var point in innerPoints)
                point.Draw(g);
            pictureBox1.Invalidate();
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();
            InitPrimitives();
        }

        private int SupportingLine(Point2D s, int startInd, classifyEnum side)
        {
            int dir = -1;
            if (side == classifyEnum.LEFT)
            {
                dir *= -1;
            }

            int i = startInd;
            int prevInd = i;
            Point2D a = polygon[i];
            Point2D b = polygon[i];
            i += dir;
            if (dir == 1)
            {
                i = i % polygon.Count;
            }
            else if (dir == -1 && i == -1)
            {
                i = polygon.Count - 1;
            }
            b = polygon[i];

            classifyEnum c = b.classify(s, a);
            while ((c==side) || (c==classifyEnum.BEYOND) || (c==classifyEnum.BETWEEN))
            {
                prevInd = i;
                i += dir;
                a = b;

                if (dir == 1)
                {
                    i = i % polygon.Count;
                }
                else if (dir == -1 && i == -1)
                {
                    i = polygon.Count - 1;
                }
                b = polygon[i];
                c = b.classify(s, a);
                
            }
            //g.DrawLine(Pens.Red, s.ToPoint(), polygon[i].ToPoint());
            return prevInd;
        }

        int closestVertex(Point2D point)
        {
            var minDist = double.MaxValue;
            int res = 0;
            for (int i = 0; i < polygon.Count; i++)
            {
                if (point.DistanceTo(polygon[i]) < minDist)
                {
                    minDist = point.DistanceTo(polygon[i]);
                    res = i;
                }
            }
            return res;
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            Clear();
            var point = new Point2D(e.X, e.Y);
            if (polygon.Count < 3)
                polygon.AddNextPoint(point);
            else
            {
                if (point.IsInsidePolygon(polygon))
                {
                    innerPoints.Add(point);
                }
                else
                {
                    var closestPoint = polygon[closestVertex(point)];
                    int left = 0;
                    int right = 0;
                    
                    for (int j = 0; j < polygon.Count; j++)
                    {
                        Edge2D edge = new Edge2D(point, polygon[j]);
                        bool flag = true;
                        for (int i = 0; i < polygon.Count; i++)
                        {

                            if (polygon[i].CompareToEdge2(edge) > 0 && i != j)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            left = j;
                            break;
                        }
                    }

                    for (int j = 0; j < polygon.Count; j++)
                    {
                        Edge2D edge = new Edge2D(point, polygon[j]);
                        bool flag = true;
                        for (int i = 0; i < polygon.Count; i++)
                        {

                            if (polygon[i].CompareToEdge2(edge) < 0 && i != j)
                            {
                                flag = false;
                                break;
                            }
                        }
                        if (flag)
                        {
                            right = j;
                            break;
                        }
                    }
                    g.DrawLine(Pens.Blue, point.ToPoint(), closestPoint.ToPoint());
                    g.DrawLine(Pens.Green, point.ToPoint(), polygon[left].ToPoint());
                    g.DrawLine(Pens.Red, point.ToPoint(), polygon[right].ToPoint());

                    (Polygon p1, Polygon p2) = polygon.Split(left, right);

                    Pen pen = new Pen(p1.Color);
                    for (int i = 0; i < p1.Count - 1; i++)
                    {
                        g.DrawLine(pen, p1[i].X, p1[i].Y, p1[i + 1].X, p1[i + 1].Y);
                    }

                    pen = new Pen(p2.Color);
                    for (int i = 0; i < p2.Count - 1; i++)
                    {
                        g.DrawLine(pen, p2[i].X, p2[i].Y, p2[i + 1].X, p2[i + 1].Y);
                    }

                    Polygon p3 = p1;
                    if (p1.Center.DistanceTo(point) < p2.Center.DistanceTo(point))
                    {
                        p3 = p2;
                        for (int i = 0; i < p1.Count; i++)
                            if (polygon[left]!= p1[i] && polygon[right] != p1[i])
                                innerPoints.Add(p1[i]);
                    }
                    else
                    {
                        for (int i = 0; i < p2.Count; i++)
                            if (polygon[left] != p2[i] && polygon[right] != p2[i])
                                innerPoints.Add(p2[i]);
                    }
                    p3.AddNextPoint(point);
                    p3.Color = Color.Black;
                    polygon = p3;
                }
            }
            Draw();
            pictureBox1.Image = bitmap;
        }
    }
}
