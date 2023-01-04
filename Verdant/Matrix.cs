using System;

namespace Verdant
{
    /// <summary>
    /// A Matrix data type.
    /// </summary>
    public class Matrix
    {

        // The number of rows in the matrix.
        public int Rows { get; private set; }
        // The number of columns in the matrix.
        public int Columns { get; private set; }
        // The values in the matrix.
        public float[,] Data { get; set; }

        /// <summary>
        /// Initialize a new Matrix.
        /// </summary>
        /// <param name="r">The number of rows.</param>
        /// <param name="c">The number of columns.</param>
        public Matrix(int r, int c)
        {
            Rows = r;
            Columns = c;
            Data = new float[r, c];
        }

        public static Vec2 operator *(Matrix m, Vec2 v)
        {
            Vec2 result = new Vec2(0, 0);
            result.X = m.Data[0, 1] * v.X + m.Data[0, 1] * v.Y;
            result.Y = m.Data[1, 0] * v.X + m.Data[1, 1] * v.Y;
            return result;
        }

    }
}
