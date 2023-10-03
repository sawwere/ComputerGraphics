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

namespace Lab4
{
    enum Mode { POINT, EDGE, POLYGON };

    public partial class Form1 : Form
    {
        private Mode currentMode = Mode.POINT;
        private Graphics g;
        private Bitmap bitmap;

        private Polygon polygon;
        private Point2D point;
        private Edge2D edge;
        private int edgeCurrentPointer;

        private Point2D customPoint;
        private Edge2D customEdge;
        private int customEdgePointer;

        public Form1()
        {
            InitializeComponent();
            g = this.CreateGraphics();
            g.Clear(Color.White);
            bitmap = new Bitmap(pictureBox1.Image);
            g = Graphics.FromImage(bitmap);
            modeComboBox.SelectedIndex = 2;
            InitPrimitives();
        }

        private void InitPrimitives()
        {
            switch (currentMode)
            {
                case Mode.POINT:
                    {
                        point = new Point2D(0, 0);
                        break;
                    }
                case Mode.EDGE:
                    {
                        edgeCurrentPointer = 0;
                        edge = new Edge2D(new Point2D(0, 0), new Point2D(0, 0));
                        break;
                    }
                case Mode.POLYGON:
                    {
                        polygon = new Polygon();
                        break;
                    }
            }
            customPoint = new Point2D(0, 0, Color.Red);
            customEdgePointer = 0;
            customEdge = new Edge2D(new Point2D(0, 0), new Point2D(0, 0), Color.Blue);
        }

        private void modeComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            currentMode = (Mode)modeComboBox.SelectedIndex;
            InitPrimitives();
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

            if (e.Button == MouseButtons.Right)
            {
                // затираем прошлую точку
                g.DrawRectangle(new Pen(Color.White), customPoint.X, customPoint.Y, 1, 1);
                customPoint.X = e.X;
                customPoint.Y = e.Y;
                labelCustomPosition.Text = $"{e.X}; {e.Y}";
            }
            else if (e.Button == MouseButtons.Middle)
            {
                // затираем прошлый отрезок
                g.DrawLine(new Pen(Color.White), customEdge.Point1.ToPoint(), customEdge.Point2.ToPoint());
                if (customEdgePointer == 1)
                    customEdge.Point2 = new Point2D(e.X, e.Y);
                else
                    customEdge.Point1 = new Point2D(e.X, e.Y);
                customEdgePointer++;
                customEdgePointer %= 2;
            }
            else
            {
                Clear();
                switch (currentMode)
                {
                    case Mode.POINT:
                        {
                            point.X = e.X;
                            point.Y = e.Y;
                            break;
                        }
                    case Mode.EDGE:
                        {
                            if (edgeCurrentPointer == 1)
                                edge.Point2 = new Point2D(e.X, e.Y);
                            else
                                edge.Point1 = new Point2D(e.X, e.Y);
                            edgeCurrentPointer++;
                            edgeCurrentPointer %= 2;
                            break;
                        }
                    case Mode.POLYGON:
                        {
                            polygon.AddNextPoint(new Point2D(e.X, e.Y));
                            break;
                        }
                }
            }
            Draw();
            pictureBox1.Image = bitmap;
        }

        private void Clear()
        {
            g.Clear(Color.White);
            pictureBox1.Image = bitmap;
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            Clear();
            InitPrimitives();
        }

        private void Draw()
        {
            switch (currentMode)
            {
                case Mode.POINT:
                    point.Draw(g);
                    break;
                case Mode.EDGE:
                    edge.Draw(g);
                    break;
                case Mode.POLYGON:
                    polygon.Draw(g);
                    break;
            }
            customPoint.Draw(g);
            customEdge.Draw(g);
            pictureBox1.Invalidate();
        }

        private void buttonClassifyCustomPointPos_Click(object sender, EventArgs e)
        {
            StringBuilder res = new StringBuilder();
            switch (currentMode)
            {
                case Mode.POINT:
                    int dx = customPoint.CompareByX(point);
                    if (dx < 0)
                        res.Append("Левее, ");
                    else if (dx > 0)
                        res.Append("Правее, ");
                    else
                        res.Append("Равны по х, ");
                    int dy = customPoint.CompareByY(point);
                    dy *= -1; // Так как рисуем сверху вниз, а значит положения точек по у противоположны настоящим
                    if (dy < 0)
                        res.Append("Ниже");
                    else if (dy > 0)
                        res.Append("Выше");
                    else
                        res.Append("Равны по y");
                    break;
                case Mode.EDGE:
                    int cmp = customPoint.CompareToEdge(edge);
                    cmp *= -1; // Так как рисуем сверху вниз, а значит положения точек по у противоположны настоящим
                    if (cmp < 0)
                        res.Append("Ниже");
                    else if (cmp > 0)
                        res.Append("Выше");
                    else
                        res.Append("Дежит на отрезке");
                    break;
                case Mode.POLYGON:
                    if (customPoint.IsInsidePolygon(polygon))
                        res.Append("Внутри");
                    else
                        res.Append("Снаружи");
                    break;
            }
            labelClassifyCustomPointPos.Text = res.ToString();
        }
    }
}
