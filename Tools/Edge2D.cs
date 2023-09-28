using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tools
{
    public class Edge2D : IColorable
    {
        public Color Color { get; set; }
        public Point2D Point1 { get; set; }
        public Point2D Point2 { get; set; }

        public Edge2D(Point2D p1, Point2D p2)
        {
            Point1 = p1;
            Point2 = p2;
            Color = Color.Black;
        }

        public Edge2D(Point2D p1, Point2D p2, Color c)
        {
            Point1 = p1;
            Point2 = p2;
            Color = c;
        }

        public void Draw(Graphics g)
        {
            Pen pen = new Pen(Color);
            g.DrawLine(pen, Point1.X, Point1.Y, Point2.X, Point2.Y);
        }
    }
}
