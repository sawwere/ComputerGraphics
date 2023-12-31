﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Tools.Primitives
{
    public class Point3D: IEquatable<Point3D>
    {
        private static PointF DEFAULT_TEXTURE_POINT = new PointF(0, 0);

        public float X;
        public float Y;
        public float Z;
        public float illumination;
        public PointF TextureCoordinates;

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
            illumination = 1.0f;
            TextureCoordinates = new PointF(0, 0);
            //нужно ли?
            //TextureCoordinates = DEFAULT_TEXTURE_POINT;
        }

        public Point3D(float x, float y, float z, float ilum)
        {
            X = x;
            Y = y;
            Z = z;
            illumination = ilum;
            TextureCoordinates = new PointF(0, 0);
            //нужно ли?
            //TextureCoordinates = DEFAULT_TEXTURE_POINT;
        }

        public Point3D(float x, float y, float z, float ilum, PointF textureCoordinates)
        {
            X = x;
            Y = y;
            Z = z;
            illumination = 1.0f;
            TextureCoordinates = textureCoordinates;
        }

        public Point3D Clone()
        {
            return new Point3D(X, Y, Z, illumination, TextureCoordinates);
        }

        public void Translate(float dx, float dy, float dz)
        {
            float[][] xyz = new float[1][] 
            { 
                new float[4] { X, Y, Z, 1 }
            };
            float[][] c = MatrixFactory.MatrixProduct(xyz, MatrixFactory.MatrixTranslate(dx, dy, dz));

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
            X = c[0][0];
            Y = c[0][1];
            Z = c[0][2];
        }

        public void Apply(float[][] matr)
        {
            float[][] xyz = new float[1][]
            {
                new float[4] { X, Y, Z, 1 }
            };
            float[][] c = MatrixFactory.MatrixProduct(xyz, matr);
            X = c[0][0];
            Y = c[0][1];
            Z = c[0][2];
        }

        /// <summary>
        /// Нормированный вектор
        /// </summary>
        public Point3D Normalize()
        {
            return ( 1 / this.Length) * this;
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

        public Point3D GetPerspective(Scene.Camera camera)
        {
            float[][] xyz = new float[1][]
            {
                new float[4] { X, Y, Z, 1 }
            };
            float[][] c = MatrixFactory.MatrixProduct(xyz, MatrixFactory.MatrixPerspective(camera));
            c = MatrixFactory.MatrixProduct(c, 1 / c[0][3]);
            return new Point3D(c[0][0], c[0][1], c[0][2]);
        }

        public PointF GetPerspectiveProj(Scene.Camera camera)
        {
            Point3D p = GetPerspective(camera);
            if (p.Z > 1)
                return new PointF(0, 0);
            return new PointF(p.X*camera.Width, p.Y*camera.Height);
        }

        public Point3D GetOrthographic(Axis a)
        {
            float[][] xyz = new float[1][]
            {
                new float[4] { X, Y, Z, 1 }
            };
            float[][] c = MatrixFactory.MatrixProduct(xyz, MatrixFactory.MatrixOrthographic(a));
            c = MatrixFactory.MatrixProduct(c, 1 / c[0][3]);

            if (a == Axis.AXIS_X)
                return new Point3D(c[0][1] , c[0][2], 0 );
            else if (a == Axis.AXIS_Y)
                return new Point3D(c[0][0] , c[0][2], 0);
            else
                return new Point3D(c[0][0] , c[0][1], 0);
        }

        public PointF GetOrthographicProj(Axis a)
        {
            Point3D p = GetOrthographic(a);
            return new PointF(p.X * 100, p.Y * 100);
        }

        public Point3D GetIsometric()
        {
            float[][] xyz = new float[1][]
            {
                new float[4] { X, Y, Z, 1 }
            };
            float[][] c = MatrixFactory.MatrixProduct(xyz, MatrixFactory.MatrixIsometric());
            c = MatrixFactory.MatrixProduct(c, 1 / c[0][3]);

            return new Point3D (c[0][0] , c[0][1], c[0][2]);
        }

        public PointF GetIsometricProj()
        {
            Point3D p = GetIsometric();
            return new PointF(p.X * 100, p.Y * 100);
        }

        public override string ToString()
        { 
            return $"{X} { Y} { Z}";
        }
    }
}
