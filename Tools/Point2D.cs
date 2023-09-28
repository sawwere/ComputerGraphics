using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tools
{
    public class Point2D : IColorable
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

        /// <summary>
        ///  Сравнивает значение координат Y данной точки и точки other
        /// </summary>
        /// <returns> < 0 - ниже;  0 - выше; == 0 - равны</returns>
        public int CompareByY(Point2D other)
        {
            return Y - other.Y;
        }

        /// <summary>
        /// Сравнивает значение координат X данной точки и точки other
        /// </summary>
        /// <returns> < 0 - правее; > 0 - левее; == 0 - равны</returns>
        public int CompareByX(Point2D other)
        {
            return X - other.X;
        }
    }
}
