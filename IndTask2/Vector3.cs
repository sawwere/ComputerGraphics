using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndTask2
{
    public class Vector3
    {
        public float X;
        public float Y;
        public float Z;

        public float Length
        {
            get
            {
                return (float)Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        public Vector3(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3 Clone()
        {
            return new Vector3(X, Y, Z);
        }

        public override string ToString()
        {
            return $"{X} {Y} {Z}";
        }

        public static Vector3 operator -(Vector3 p1, Vector3 p2)
        {
            return new Vector3(p1.X - p2.X, p1.Y - p2.Y, p1.Z - p2.Z);

        }

        /// <summary>
        /// Векторное произведение векторов
        /// </summary>
        public Vector3 CrossProduct(Vector3 other)
        {
            return new Vector3(Y * other.Z - Z * other.Y, Z * other.X - X * other.Z, X * other.Y - Y * other.X);
        }

        /// <summary>
        /// Скалярное произведение: вектор на вектор
        /// </summary>
        public float DotProduct(Vector3 other)
        {
            return X * other.X + Y * other.Y + Z * other.Z;
        }

        /// <summary>
        /// Нормированный вектор
        /// </summary>
        public Vector3 Normalize()
        {
            if (Length == 0)
                return new Vector3(0, 0, 0);
            return (1 / this.Length) * this;
        }

        public static Vector3 operator +(Vector3 p1, Vector3 p2)
        {
            return new Vector3(p1.X + p2.X, p1.Y + p2.Y, p1.Z + p2.Z);
        }

        public static Vector3 operator *(float t, Vector3 p1)
        {
            return new Vector3(p1.X * t, p1.Y * t, p1.Z * t);
        }


        public static Vector3 operator *(Vector3 p1, float t)
        {
            return new Vector3(p1.X * t, p1.Y * t, p1.Z * t);
        }

        public static Vector3 operator /(Vector3 p1, float t)
        {
            return new Vector3(p1.X / t, p1.Y / t, p1.Z / t);
        }

        public static Vector3 operator /(float t, Vector3 p1)
        {
            return new Vector3(t / p1.X, t / p1.Y, t / p1.Z);
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

        public void Rotate(double angle, MainForm.Axis a)
        {
            float[][] xyz = new float[1][]
            {
                new float[4] { X, Y, Z, 1 }
            };
            float[][] c = MatrixFactory.MatrixProduct(xyz, MatrixFactory.MatrixRotate(angle, a));
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
    }
}
