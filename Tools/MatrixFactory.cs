using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Tools.Primitives;

namespace Tools
{
    static class MatrixFactory
    {
        /// <summary>
        /// Матриица перехода к перспективной проекции
        /// </summary>
        public static float[][] MatrixPerspective(Scene.Camera camera)
        {
            float k = 10;
            float aspectRatio = camera.Width / (float)camera.Height;
            float n = 1;
            float f = 10;
            //var res = new float[4][]
            //{ new float[4] {1,  0,  0,  0},
            //  new float[4] {0,  1,  0,  0},
            //  new float[4] {0,  0,  1,  -1/k},
            //  new float[4] {0,  0,  0,  1}
            //};
            var res = new float[4][]
            { new float[4] {(float)(1/(Math.Tan(camera.fovy / 2) * aspectRatio)),  0,  0,  0},
              new float[4] {0, (float)(1 / Math.Tan(camera.fovy / 2)),  0,  0},
              new float[4] {0,  0,  (f+n)/(f-n),  2*f*n/(f-n)},
              new float[4] {0,  0,  1, 0}
            };
            return res;
        }

        /// <summary>
        /// Матриица перехода к ортографической проекции
        /// </summary>
        public static float[][] MatrixOrthographic(Axis a)
        {
            var res = new float[4][]
            { new float[4] {1,  0,  0,  0},
              new float[4] {0,  1,  0,  0},
              new float[4] {0,  0,  1,  0},
              new float[4] {0,  0,  0,  1}
            };
            if (a == Axis.AXIS_X)
                res[0][0] = 0.0f;
            else if (a == Axis.AXIS_Y)
                res[1][0] = 0.0f;
            else
                res[2][0] = 0.0f;
            return res;
        }

        /// <summary>
        /// Матриица перехода к изометрической проекции
        /// </summary>
        public static float[][] MatrixIsometric()
        {
            double r_phi = Math.PI / 4;
            double r_psi = Math.PI / 4;
            float cos_phi = (float)Math.Cos(r_phi);
            float sin_phi = (float)Math.Sin(r_phi);
            float cos_psi = (float)Math.Cos(r_psi);
            float sin_psi = (float)Math.Sin(r_psi);
            var res = new float[4][]
            { new float[4] { cos_phi, sin_phi * sin_psi,   0,  0},
              new float[4] { 0,       cos_phi,             0,  0},
              new float[4] { sin_psi, -sin_phi * cos_psi,  0,  0},
              new float[4] { 0,       0,                   0,  1}
            };
            return res;
        }

        /// <summary>
        /// Матриица аффинного преобразования - перенос
        /// </summary>
        public static float[][] MatrixTranslate(float tx, float ty, float tz)
        {
            return new float[4][]
            { new float[4] {1,  0,  0,  0},
              new float[4] {0,  1,  0,  0},
              new float[4] {0,  0,  1,  0},
              new float[4] {tx, ty, tz, 1}
            };
        }
        /// <summary>
        /// Матриица аффинного преобразования - масштабирование
        /// </summary>
        public static float[][] MatrixScale(float kx, float ky, float kz)
        {
            return new float[4][]
            { new float[4] {kx,  0,  0,  0},
              new float[4] {0,  ky,  0,  0},
              new float[4] {0,  0,  kz,  0},
              new float[4] {0, 0, 0, 1}
            };
        }
        /// <summary>
        /// Матриица аффинного преобразования - поворот
        /// </summary>
        public static float[][] MatrixRotate(double angle, Axis a, Edge3D line = null)
        {
            double rangle = Math.PI * angle / 180;
            float sin = (float)Math.Sin(rangle);
            float cos = (float)Math.Cos(rangle);

            var res = new float[4][];
            switch (a)
            {
                case Axis.AXIS_X:
                    res = new float[4][] { 
                        new float[4] { 1,   0,     0,   0 },
                        new float[4] { 0,  cos,   sin,  0 },
                        new float[4] { 0,  -sin,  cos,  0 },
                        new float[4] { 0,   0,     0,   1 } 
                    };
                    break;
                case Axis.AXIS_Y:
                    res = new float[4][] {
                        new float[4] { cos,   0,     -sin,  0 },
                        new float[4] { 0,     1,     0,     0 },
                        new float[4] { sin,   0,     cos,   0 },
                        new float[4] { 0,     0,     0,     1 }
                    };
                    break;
                case Axis.AXIS_Z:
                    res = new float[4][] {
                        new float[4] { cos,   sin,   0,    0 },
                        new float[4] { -sin,  cos,   0,    0 },
                        new float[4] { 0,     0,     1,    0 },
                        new float[4] { 0,     0,     0,    1 }
                    };
                    break;
                case Axis.CUSTOM:
                    float l = line.Destination.X - line.Origin.X;
                    float m = line.Destination.Y - line.Origin.Y;
                    float n = line.Destination.Z - line.Origin.Z;
                    float length = (float)Math.Sqrt(l * l + m * m + n * n);
                    l /= length; m /= length; n /= length;
                    res = new float[4][] {
                        new float[4] {l * l + cos * (1 - l * l),   l * (1 - cos) * m + n * sin,   l * (1 - cos) * n - m * sin,  0 },
                        new float[4] { l * (1 - cos) * m - n * sin,   m * m + cos * (1 - m * m),    m * (1 - cos) * n + l * sin,  0 },
                        new float[4] { l * (1 - cos) * n + m * sin,  m * (1 - cos) * n - l * sin,    n * n + cos * (1 - n * n),   0 },
                        new float[4] { 0,     0,     0,    1 }
                    };
                    break;
            }
            return res;
        }

        /// <summary>
        /// Создаем матрицу, полностью инициализированную значениями 0.0
        /// </summary>
        public static float[][] MatrixCreate(int rows, int cols)
        {
            // 
            float[][] result = new float[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new float[cols];
            return result;
        }

        /// <summary>
        /// Умножение 2 матриц
        /// </summary>
        public static float[][] MatrixProduct(float[][] matrixA, float[][] matrixB)
        {
            int aRows = matrixA.Length; 
            int aCols = matrixA[0].Length;
            int bRows = matrixB.Length; 
            int bCols = matrixB[0].Length;
            if (aCols != bRows)
                throw new Exception("Matrices cannot be multiplied");
            float[][] result = MatrixCreate(aRows, bCols);
            Parallel.For(0, aRows, i =>
                {
                    for (int j = 0; j < bCols; ++j)
                        for (int k = 0; k < aCols; ++k)
                            result[i][j] += matrixA[i][k] * matrixB[k][j];
                });
            return result;
        }

        /// <summary>
        /// Умножение матрицы на число
        /// </summary>
        public static float[][] MatrixProduct(float[][] matrixA, float b)
        {
            int aRows = matrixA.Length;
            int aCols = matrixA[0].Length;
            float[][] result = MatrixCreate(aRows, aCols);
            Parallel.For(0, aRows, i =>
            {
                for (int j = 0; j < aCols; ++j)
                    result[i][j] += matrixA[i][j] * b;
            });
            return result;
        }
    }
}
