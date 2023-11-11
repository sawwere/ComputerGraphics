using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tools
{
    public class Polygon : IPrimitive
    {
        public Color Color { get; set; }

        /// <returns>X координата самой левой точки</returns>
        public float X 
        {
            get { return this.points.OrderBy(_x => _x.X).First().X; }
        }

        /// <returns>Координата Y данного примитива - 
        /// Y координата самой верхней(относительно отображения в winform) его точки</returns>
        public float Y 
        {
            get { return this.points.OrderBy(_x => _x.Y).First().Y; }
        }

        /// <returns>X координата самой левой точки</returns>
        public float Left
        {
            get { return X; }
        }

        /// <returns>X координата самой правой точки</returns>
        public float Right
        {
            get { return this.points.OrderBy(_x => _x.X).Last().X; }
        }
        /// <returns>Y координата самой верхней точки</returns>
        public float Top
        {
            get { return Y; }
        }
        /// <returns>Y координата самой нижней точки</returns>
        public float Bottom
        {
            get { return this.points.OrderBy(_x => _x.Y).Last().X; }
        }

        /// <returns>Количество вершин</returns>
        public int Count
        {
            get { return this.points.Count; }
        }

        public Point2D Center
        {
            get
            {
                float sx = this.points.Select(x => x.X).Sum();
                float sy = this.points.Select(x => x.Y).Sum();
                return new Point2D(sx / this.points.Count, sy / points.Count);
            }
        }

        private List<Point2D> points;

        public Point2D this[int i]
        {
            get { return points[i]; }
            private set 
            {
                if (value is null)
                    throw new ArgumentNullException();
                points[i] = value; 
            }
        }

        private void Initialize(List<Point2D> points, Color color)
        {
            Color = color;
            this.points = new List<Point2D>();
            if (points is null)
                return;
            foreach (var point in points)
                AddNextPoint(point);
        }

        public Polygon()
        {
            Initialize(null, Color.Black);
        }

        public Polygon(Color color)
        {
            Initialize(null, color);
        }

        public Polygon(List<Point2D> points)
        {
            Initialize(points, Color.Black);
        }

        public Polygon(List<Point2D> points, Color color)
        {
            Initialize(points, color);
        }

        public void AddNextPoint(Point2D point)
        {
            points.Add(point);
        }

        public void AddNextPoint(PointF point)
        {
            AddNextPoint(new Point2D(point));
        }

        public void RemoveLastPoint()
        {
            if (points.Count == 0)
                return;
            points.RemoveAt(points.Count - 1);
        }

        public void Clear()
        {
            points.Clear();
        }

        public void Draw(Graphics g)
        {
            if (points.Count < 1)
                return;
            Pen pen = new Pen(Color);
            for (int i = 0; i < points.Count - 1; i++)
            {
                g.DrawLine(pen, points[i].X, points[i].Y, points[i + 1].X, points[i + 1].Y);
            }

            //g.DrawLine(pen, points[0].X, points[0].Y, points.Last().X, points.Last().Y);
        }

        public (Polygon, Polygon) Split(int left, int right)
        {
            Polygon p1 = new Polygon(Color.Red);
            Polygon p2 = new Polygon(Color.Green);
            if (left > right)
                (left, right) = (right, left);
            for (int i = left; i <= right; i++)
                p1.AddNextPoint(points[i]);

            for (int i = right; i != left; i = (i+1)%points.Count)
            {
                p2.AddNextPoint(points[i]);
            }
            p2.AddNextPoint(points[left]);

            return (p1, p2);
        }
    }
}
