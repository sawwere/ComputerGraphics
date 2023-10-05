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

        public void DrawWide(Graphics g, int width = 1)
        {
            Pen pen = new Pen(Color);
            g.DrawRectangle(pen, X - width / 2, Y - width / 2, width, width);
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
        /// <returns>меньше 0 - левее; больше 0 - Правее; == 0 - равны</returns>
        public int CompareByX(Point2D other)
        {
            return X - other.X;
        }

        /// <summary>
        /// Сравнивает положение точки и отрезка
        /// </summary>
        /// <returns>меньше 0 - ниже; больше 0 - выше; == 0 - на отрезке</returns>
        public int CompareToEdge(Edge2D edge) // TODO
        {
            int val = (X - edge.Point1.X) * (edge.Point2.Y - edge.Point1.Y) 
                - (Y - edge.Point1.Y) * (edge.Point2.X - edge.Point1.X);
            return val;
        }

        /// <summary>
        /// Расстояние до точки other
        /// </summary>
        public double DistanceTo(Point2D other)
        {
            return Math.Sqrt((X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y));
        }

        

        /// <summary>
        /// Находится ли точка внутри полигона
        /// </summary>
        public bool IsInsidePolygon(Polygon polygon) // TODO
        {
            int parity = 0;

            for (int i = 0; i < polygon.Count - 1; i++)
            {
                Edge2D edge = new Edge2D(polygon[i], polygon[i + 1]);

                parity = 1 - parity;

            }
            return (parity == 1 ? true : false);
        }

        public static Point2D operator+(Point2D ths, Point2D other)
        {
            return new Point2D(ths.X + other.X, ths.X + other.Y);
        }

        public static Point2D operator -(Point2D ths, Point2D other)
        {
            return new Point2D(ths.X - other.X, ths.X - other.Y);
        }
    }
}
