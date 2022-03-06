using System;

namespace IsoEngine
{
    public class Matrix
    {

        public int Rows { get; set; }
        public int Columns { get; set; }
        public float[,] Data { get; set; }

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
