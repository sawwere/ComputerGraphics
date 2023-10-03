using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class Point2D : IPrimitive
    {
        public Color Color { get; set; }
        public int X { get; set; }
        public int Y { get; set; }

        public Point2D(int x, int y)
        {
            X = x;
            Y = y;
            Color = Color.Black;
        }

        public Point2D(int x, int y, Color c)
        {
            X = x;
            Y = y;
            Color = c;
        }

        public Point2D(Point point)
        {
            X = point.X;
            Y = point.Y;
            Color = Color.Black;
        }

        public Point2D(Point point, Color c)
        {
            X = point.X;
            Y = point.Y;
            Color = c;
        }

        public void Draw(Graphics g)
        {
            Pen pen = new Pen(Color);
            g.DrawRectangle(pen, X, Y, 1, 1);
        }

        public Point ToPoint()
        {
            return new Point(X, Y);
        }

        /// <summary>
        /// Сравнивает значение координат Y данной точки и точки other
        /// </summary>
        /// <returns>меньше 0 - ниже;  больше 0 - выше; == 0 - равны</returns>
        public int CompareByY(Point2D other)
        {
            return Y - other.Y;
        }

        /// <summary>
        /// Сравнивает значение координат X данной точки и точки other
        /// </summary>
        /// <returns>меньше 0 - Левее; больше 0 - Правее; == 0 - равны</returns>
        public int CompareByX(Point2D other)
        {
            return X - other.X;
        }

        /// <summary>
        /// Сравнивает положение точки и отрезка
        /// </summary>
        /// <returns>меньше 0 - ниже; больше 0 - выше; == 0 - на отрезке</returns>
        public int CompareToEdge(Edge2D edge)
        {
            int val = (X - edge.Point1.X) * (edge.Point2.Y - edge.Point1.Y) 
                - (Y - edge.Point1.Y) * (edge.Point2.X - edge.Point1.X);
            return val;
        }

        public bool IsInsidePolygon(Polygon polygon)
        {
            return false;
        }
    }
}
