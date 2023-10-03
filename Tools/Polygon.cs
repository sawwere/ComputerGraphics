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

        public int X 
        { 
            get; 
            private set; 
        }
        public int Y 
        { 
            get; 
            private set; 
        }

        private List<Point2D> points;

        public Point2D this[int i]
        {
            get { return points[i]; }
            set { points[i] = value; }
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
            if (point.X < X)
                X = point.X;
            if (point.Y < Y)
                Y = point.Y;
        }

        public void AddNextPoint(Point point)
        {
            AddNextPoint(new Point2D(point));
        }

        public void RemoveLastPoint()
        {
            if (points.Count == 0)
                return;
            points.RemoveAt(points.Count - 1);
            X = points.Min(x => x.X);
            Y = points.Min(x => x.Y);
        }

        public void Clear()
        {
            points.Clear();
        }

        public void Draw(Graphics g)
        {
            if (points.Count < 1)
                return;
            Pen pen = new Pen(Color.Black);
            for (int i = 0; i < points.Count - 1; i++)
            {
                g.DrawLine(pen, points[i].X, points[i].Y, points[i + 1].X, points[i + 1].Y);
            }

            g.DrawLine(pen, points[0].X, points[0].Y, points.Last().X, points.Last().Y);
        }
    }
}
