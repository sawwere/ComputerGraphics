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
            /*private set;*/ 
        }
        public int Y
        {
            get { return Math.Min(Point1.Y, Point2.Y); }
            /*private set;*/
        }
        public Point2D Point1 { get; set; }
        public Point2D Point2 { get; set; }

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
    }
}
