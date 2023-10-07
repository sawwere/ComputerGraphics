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
                        res.Append("Лежит на отрезке");
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

        private void buttonAction_Click(object sender, EventArgs e)
        {
            if (radioButtonMove.Checked)
            {
                switch (currentMode)
                {
                    case Mode.POLYGON:
                        {
                            int offsetX = customPoint.X - polygon.Center.X;
                            int offsetY = customPoint.Y - polygon.Center.Y;
                            for (int i = 0; i < polygon.Count; i++)
                            {
                                var pf = Translate(polygon[i], offsetX, offsetY);
                                polygon[i].X = (int)pf.X;
                                polygon[i].Y = (int)pf.Y;
                            }
                            break;
                        }
                    case Mode.EDGE:
                        {
                            int offsetX = customPoint.X - edge.Center.X;
                            int offsetY = customPoint.Y - edge.Center.Y;
                            var pf = Translate(edge.Point1, offsetX, offsetY);
                            edge.Point1.X = (int)pf.X;
                            edge.Point1.Y = (int)pf.Y;
                            pf = Translate(edge.Point2, offsetX, offsetY);
                            edge.Point2.X = (int)pf.X;
                            edge.Point2.Y = (int)pf.Y;
                            break;
                        }
                    case Mode.POINT:
                        {
                            int offsetX = customPoint.X - point.X;
                            int offsetY = customPoint.X - point.X;
                            var pf = Translate(point, offsetX, offsetY);
                            point.X = (int)pf.X;
                            point.Y = (int)pf.Y;
                            break;
                        }
                }
            }
            else if (radioButtonRotate.Checked)
            {
                double rotateAngle = (double)numericUpDownRotate.Value;
                var rotatePoint = customPoint;
                switch (currentMode)
                {
                    case Mode.POLYGON:
                        {
                            if (checkBoxCenterRotate.Checked)
                                rotatePoint = polygon.Center;
                            for (int i = 0; i < polygon.Count; i++)
                            {
                                var pf = Rotate(polygon[i], rotatePoint, rotateAngle);
                                polygon[i].X = (int)pf.X;
                                polygon[i].Y = (int)pf.Y;
                            }
                            break;
                        }
                }
            }
            else if (radioButtonScale.Checked)
            {
                switch (currentMode)
                {
                    case Mode.POLYGON:
                        {
                            for (int i = 0; i < polygon.Count; i++)
                            {
                                int scaleX = (int)numericUpDownScaleX.Value;
                                int scaleY = (int)numericUpDownScaleY.Value;
                                var pf = Scale(polygon[i], scaleX, scaleY);
                                polygon[i].X = (int)pf.X;
                                polygon[i].Y = (int)pf.Y;
                            }
                            break;
                        }
                }
            }
            Clear();
            Draw();
        }

        private List<Point2D> FindIntersections()
        {
            List<Point2D> res = new List<Point2D>();
            switch (currentMode)
            {
                case Mode.POINT: break;
                case Mode.EDGE:
                    {
                        var ir = edge.Intersect(customEdge);
                        if (ir.Item2 == EdgeIntersectionType.INTERSECT) 
                        {
                            res.Add(ir.Item1);
                            break;
                        }

                        break;
                    }
                case Mode.POLYGON:
                    {
                        Edge2D e = new Edge2D(polygon[0], polygon[polygon.Count - 1]);
                        var ir = e.Intersect(customEdge);
                        if (ir.Item2 == EdgeIntersectionType.INTERSECT)
                        {
                            res.Add(ir.Item1);
                        }
                        for (int i = 0; i < polygon.Count - 1; i++)
                        {
                            e = new Edge2D(polygon[i], polygon[i + 1]);
                            ir = e.Intersect(customEdge);
                            if (ir.Item2 == EdgeIntersectionType.INTERSECT)
                            {
                                res.Add(ir.Item1);
                            }
                        }
                        break;
                    }

            }

            return res;
        }

        private void buttonFindIntersections_Click(object sender, EventArgs e)
        {
            var pi = FindIntersections();
            foreach (var point in pi)
                point.DrawWide(g, 3);
            pictureBox1.Invalidate();
        }

        private Point2D Translate(Point2D pp, int offsetX, int offsetY)
        {
            int[] offsetVector = new int[3] { pp.X, pp.Y, 1 };

            int[] resultVector = new int[3];

            int[][] Matrix = new int[3][]{
                new int[3] { 1,   0, 0 },
                new int[3] { 0,   1, 0 },
                new int[3] { offsetX, offsetY, 1 } };

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    resultVector[i] += Matrix[j][i] * offsetVector[j];
            }
            return new Point2D(resultVector[0], resultVector[1]);
        }


        private PointF Rotate(Point2D point2D, Point2D rotatePoint, double rotateAngle)
        {
            PointF pointF = new PointF(point2D.X, point2D.Y);
            double pointA, pointB;
            var angle = (rotateAngle / 180D) * Math.PI;
            if (currentMode == Mode.POLYGON) // ??
            {
                pointA = -rotatePoint.X * Math.Cos(angle) + rotatePoint.Y * Math.Sin(angle) + rotatePoint.X;
                pointB = -rotatePoint.X * Math.Sin(angle) - rotatePoint.Y * Math.Cos(angle) + rotatePoint.Y;
            }
            else
            {
                pointA = -rotatePoint.X * (Math.Cos(angle) - 1) + rotatePoint.Y * Math.Sin(angle);
                pointB = -rotatePoint.X * Math.Sin(angle) - rotatePoint.Y * (Math.Cos(angle) - 1);
            }

            double[] offsetVector = new double[3] { pointF.X, pointF.Y, 1 };
            double[][] Matrix = new double[3][]{
                new double[3] {  Math.Cos(angle),   Math.Sin(angle), 0 },
                new double[3] { -Math.Sin(angle),   Math.Cos(angle), 0 },
                new double[3] { pointA, pointB, 1 } };
            double[] resultVector = new double[3];
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    resultVector[i] += offsetVector[j] * Matrix[j][i];
            }

            pointF.X = (float)resultVector[0];
            pointF.Y = (float)resultVector[1];
            return pointF;
        }

        private PointF Scale(Point2D point2D, int scaleX, int scaleY) // TODO
        {
            // переделать
            var mashtabX = (double)scaleX / 100d;
            var mashtabY = (double)scaleY / 100d;
            PointF pointF = new PointF(point2D.X, point2D.Y);
            PointF minPolyPoint, maxPolyPoint;

            double[] offsetVector = new double[3] { point2D.X, point2D.Y, 1 };
            double[,] Matrix = new double[3, 3];
            double[] resultVector = new double[3];
            
            if (checkBoxCenterScale.Checked)
            {
                pointF = new PointF((polygon.Left + polygon.Right) / 2, (polygon.Bottom + polygon.Top) / 2);
            }
            else
            {
                pointF = ;
            }
            
            Matrix[0, 0] = scaleX;
            Matrix[0, 1] = 0;
            Matrix[0, 2] = 0; 
            Matrix[1, 0] = 0;
            Matrix[1, 1] = scaleY;
            Matrix[1, 2] = 0; 
            Matrix[2, 0] = (1 - scaleX) * pointF.X;
            Matrix[2, 1] = (1 - scaleY) * pointF.Y;
            Matrix[2, 2] = 1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                    resultVector[i] += Matrix[j, i] * offsetVector[j];
            }

            pointF.X = (float)resultVector[0];
            pointF.Y = (float)resultVector[1];
            return new PointF();
        }
    }
}
