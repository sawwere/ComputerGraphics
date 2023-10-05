using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tools
{
    public class Edge2D : IPrimitive
    {
        public Color Color { get; set; }
        public int X 
        { 
            get { return Math.Min(Point1.X, Point2.X); }
        }
        public int Y
        {
            get { return Math.Min(Point1.Y, Point2.Y); }
        }

        public Point2D Center
        {
            get { return new Point2D(Math.Abs(Point1.X - Point2.X) / 2, Math.Abs(Point1.Y - Point2.Y) / 2, Color); }
        }
        public Point2D Point1 { get; set; }
        public Point2D Point2 { get; set; }
        public double Length
        {
            get { return Point1.DistanceTo(Point2); }
        }
        private void Initialize(Point2D p1, Point2D p2, Color color)
        {
            Point1 = p1;
            Point2 = p2;
            Color = color;
        }

        public Edge2D(int x1, int y1, int x2, int y2, Color c)
        {
            Initialize(new Point2D(x1, y1, c), new Point2D(x2, y2, c), c);
        }

        public Edge2D(Point2D p1, Point2D p2)
        {
            Initialize(p1, p2, Color.Black);
        }

        public Edge2D(Point2D p1, Point2D p2, Color color)
        {
            Initialize(p1, p2, color);
        }

        public void Draw(Graphics g)
        {
            Pen pen = new Pen(Color);
            if (Point1 != null && Point2 != null)
                g.DrawLine(pen, Point1.X, Point1.Y, Point2.X, Point2.Y);
        }

        public static double Distance(Point2D point1, Point2D point2)
        {
            return Math.Sqrt((point1.X - point2.X) * (point1.X - point2.X)
                + (point1.Y - point2.Y) * (point1.Y - point2.Y));
        }

        public (Point2D, EdgeIntersectionType) Intersect(Edge2D other) // TODO
        {
            double v = Point2.X - Point1.X;
            double w = Point2.Y - Point1.Y;
            double v2 = other.Point2.X - other.Point1.X;
            double w2 = other.Point2.Y - other.Point1.Y;
            double lenBlue = Math.Sqrt(v * v + w * w);
            double lenRed = Math.Sqrt(v2 * v2 + w2 * w2);
            double x = v / lenBlue;
            double y = w / lenBlue;
            double x2 = v2 / lenRed;
            double y2 = w2 / lenRed;


            double epsilon = 0.000001;

            if (Point1.X == other.Point1.X 
                && Point1.Y == other.Point1.Y 
                && Point2.X == other.Point2.X 
                && Point2.Y == other.Point2.Y)
            {
                Console.WriteLine("Отрезки совпадают");
                return (null, EdgeIntersectionType.COLLINEAR);
            }

            if (Math.Abs(x - x2) < epsilon && Math.Abs(y - y2) < epsilon)
            {
                Console.WriteLine("Отрезки параллельны");
                return (null, EdgeIntersectionType.PARRALEL);
            }
            double t2 = (-w * other.Point1.X + w * Point1.X + v * other.Point1.Y - v * Point1.Y) / (w * v2 - v * w2);

            double t = (other.Point1.X - Point1.X + v2 * t2) / v;

            if (t < 0 || t > 1 || t2 < 0 || t2 > 1)
            {
                Console.WriteLine("Пересечения нет");
                return (null, EdgeIntersectionType.NOT_INTERSECT);
            }

            // Координаты точки пересечения
            var pCross = new Point2D((int)(other.Point1.X + v2 * t2), (int)(other.Point1.Y + w2 * t2), Color.Green);
            Console.WriteLine($"Пересечение {pCross.X} {pCross.Y}");
            return (pCross, EdgeIntersectionType.INTERSECT);
        }
    }
}
