using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tools.Primitives
{
    public struct Point3D: IPrimitive3D, IEquatable<Point3D>
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Color Color { get; set; }

        public float Length { get
            {
                return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        public Point3D(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
            Color = Color.Black;
        }

        public void ReflectX()
        {
            X = -X;
        }

        public void ReflectY()
        {
            Y = -Y;
        }

        public void ReflectZ()
        {
            Z = -Z;
        }

        public void Translate(float dx, float dy, float dz)
        {
            float[][] xyz = new float[1][] 
            { 
                new float[4] { X, Y, Z, 1 }
            };
            float[][] c = MatrixFactory.MatrixProduct(xyz, MatrixFactory.MatrixTranslate(dx, dy, dz));
            c = MatrixFactory.MatrixProduct(c, 1 / c[0][3]);

            X = c[0][0];
            Y = c[0][1];
            Z = c[0][2];
        }

        public void Rotate(double angle, Axis a, Edge3D line = null)
        {
            float[][] xyz = new float[1][]
            {
                new float[4] { X, Y, Z, 1 }
            };
            float[][] c = MatrixFactory.MatrixProduct(xyz, MatrixFactory.MatrixRotate(angle, a, line));
            c = MatrixFactory.MatrixProduct(c, 1 / c[0][3]);
            X = c[0][0];
            Y = c[0][1];
            Z = c[0][2];
        }

        public void Scale(float kx, float ky, float kz)
        {
            float[][] xyz = new float[1][]
            {
                new float[4] { X, Y, Z, 1 }
            };
            float[][] c = MatrixFactory.MatrixProduct(xyz, MatrixFactory.MatrixScale(kx, ky, kz));
            c = MatrixFactory.MatrixProduct(c, 1 / c[0][3]);
            X = c[0][0];
            Y = c[0][1];
            Z = c[0][2];
        }

        /// <summary>
        /// Расстояние до точки other
        /// </summary>
        public double DistanceTo(Point3D other)
        {
            return Math.Sqrt((X - other.X) * (X - other.X) + (Y - other.Y) * (Y - other.Y) + (Z - other.Z) * (Z - other.Z));
        }

        /// <summary>
        /// Расстояние до точки точки с координатами
        /// </summary>
        public double DistanceTo(float x, float y, float z)
        {
            return DistanceTo(new Point3D(x, y, z));
        }

        /// <summary>
        /// С какой стороны плоскости, задаваемой треугольником t, располагается данная точка
        /// </summary>
        public int Classify(Triangle3D t)
        {
            return 0;
        }

        /// <summary>
        /// Векторное произведение векторов
        /// </summary>
        public Point3D CrossProduct(Point3D other)
        {
            return new Point3D(Y * other.Z - Z * other.Y, Z * other.X  - X * other.Z, X * other.Y - Y * other.X);
        }

        /// <summary>
        /// Скалярное произведение: вектор на вектор
        /// </summary>
        public float DotProduct(Point3D other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }

        /// <summary>
        /// Скалярное произведение: вектор на число
        /// </summary>
        public static Point3D operator *(float k, Point3D ths)
        {
            return new Point3D(ths.X * k, ths.Y * k, ths.Z * k);
        }

        public static Point3D operator *(Point3D ths, Point3D other)
        {
            return new Point3D(ths.X * other.X, ths.Y * other.Y, ths.Z * other.Z);
        }

        public static Point3D operator +(Point3D ths, Point3D other)
        {
            return new Point3D(ths.X + other.X, ths.Y + other.Y, ths.Z + other.Z);
        }

        public static Point3D operator -(Point3D ths, Point3D other)
        {
            return new Point3D(ths.X - other.X, ths.Y - other.Y, ths.Z - other.Z);
        }

        public bool Equals(Point3D other)
        {
            return X == other.X && Y == other.Y && Z == other.Z;
        }

        public override int GetHashCode()
        {
            return new { X, Y, Z }.GetHashCode();
        }

        public PointF GetPerspective()
        {
            float k = 1000;
            if (Math.Abs(Z - k) < 1e-10)
                k += 1;

            float[][] xyz = new float[1][]
            {
                new float[4] { X, Y, Z, 1 }
            };
            float[][] c = MatrixFactory.MatrixProduct(xyz, MatrixFactory.MatrixPerspective(k));
            c = MatrixFactory.MatrixProduct(c, 1 / c[0][3]);
            return new PointF(c[0][0] , c[0][1] );
        }

        public PointF GetOrthographic(Axis a)
        {
            float[][] xyz = new float[1][]
            {
                new float[4] { X, Y, Z, 1 }
            };
            float[][] c = MatrixFactory.MatrixProduct(xyz, MatrixFactory.MatrixOrthographic(a));
            c = MatrixFactory.MatrixProduct(c, 1 / c[0][3]);

            if (a == Axis.AXIS_X)
                return new PointF(c[0][1], c[0][2]);
            else if (a == Axis.AXIS_Y)
                return new PointF(c[0][0], c[0][2]);
            else
                return new PointF(c[0][0], c[0][1]);
        }

        public PointF GetIsometric()
        {
            float[][] xyz = new float[1][]
            {
                new float[4] { X, Y, Z, 1 }
            };
            float[][] c = MatrixFactory.MatrixProduct(xyz, MatrixFactory.MatrixIsometric());
            c = MatrixFactory.MatrixProduct(c, 1 / c[0][3]);

            return new PointF(c[0][0], c[0][1]);
        }
    }
}
