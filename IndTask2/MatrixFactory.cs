using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndTask2
{
    public static class MatrixFactory
    {
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
        /// Матриица аффинного преобразования - перенос
        /// </summary>
        public static float[][] MatrixTranslate(Vector3 vec)
        {
            return MatrixTranslate(vec.X, vec.Y, vec.Z);
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
        /// Матриица аффинного преобразования - масштабирование
        /// </summary>
        public static float[][] MatrixScale(Vector3 vec)
        {
            return MatrixScale(vec.X, vec.Y, vec.Z);
        }
        /// <summary>
        /// Матриица аффинного преобразования - поворот
        /// </summary>
        public static float[][] MatrixRotate(double angle, MainForm.Axis a)
        {
            double rangle = Math.PI * angle / 180;
            float sin = (float)Math.Sin(rangle);
            float cos = (float)Math.Cos(rangle);

            var res = new float[4][];
            switch (a)
            {
                case MainForm.Axis.AXIS_X:
                    res = new float[4][] {
                        new float[4] { 1,   0,     0,   0 },
                        new float[4] { 0,  cos,   sin,  0 },
                        new float[4] { 0,  -sin,  cos,  0 },
                        new float[4] { 0,   0,     0,   1 }
                    };
                    break;
                case MainForm.Axis.AXIS_Y:
                    res = new float[4][] {
                        new float[4] { cos,   0,     -sin,  0 },
                        new float[4] { 0,     1,     0,     0 },
                        new float[4] { sin,   0,     cos,   0 },
                        new float[4] { 0,     0,     0,     1 }
                    };
                    break;
                case MainForm.Axis.AXIS_Z:
                    res = new float[4][] {
                        new float[4] { cos,   sin,   0,    0 },
                        new float[4] { -sin,  cos,   0,    0 },
                        new float[4] { 0,     0,     1,    0 },
                        new float[4] { 0,     0,     0,    1 }
                    };
                    break;
            }
            return res;
        }

        /// <summary>
        /// Матриица аффинного преобразования - поворот
        /// </summary>
        public static float[][] MatrixRotate(Vector3 vec)
        {
            float[][] res = MatrixProduct(MatrixRotate(vec.X, MainForm.Axis.AXIS_X), MatrixRotate(vec.Y, MainForm.Axis.AXIS_Y));
            res = MatrixProduct(res, MatrixRotate(vec.Z, MainForm.Axis.AXIS_Z));
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
