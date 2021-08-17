using System;

namespace IsoEngine
{
    public class TransformState
    {

        public float X { get; set; }
        public float Y { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Rotation { get; set; }

        /// <summary>
        /// Initialize a new TransformState.
        /// </summary>
        /// <param name="x">The x coordinate.</param>
        /// <param name="y">The y coordinate.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <param name="r">The rotation.</param>
        public TransformState(float x, float y, float w, float h, float r)
        {
            X = x;
            Y = y;
            Width = w;
            Height = h;
            Rotation = r;
        }

        /// <summary>
        /// Initialize a new TransformState from an existing TransformState.
        /// </summary>
        /// <param name="state">The TransformState to copy from.</param>
        public TransformState(TransformState state)
        {
            X = state.X;
            Y = state.Y;
            Width = state.Width;
            Height = state.Height;
            Rotation = state.Rotation;
        }

        public static TransformState operator +(TransformState a, TransformState b) => new TransformState(a.X + b.X, a.Y + b.Y, a.Width + b.Width, a.Height + b.Height, a.Rotation + b.Rotation);
        public static TransformState operator -(TransformState a, TransformState b) => new TransformState(a.X - b.X, a.Y - b.Y, a.Width - b.Width, a.Height - b.Height, a.Rotation - b.Rotation);
        public static TransformState operator *(TransformState a, TransformState b) => new TransformState(a.X * b.X, a.Y * b.Y, a.Width * b.Width, a.Height * b.Height, a.Rotation * b.Rotation);
        public static TransformState operator /(TransformState a, TransformState b) => new TransformState(a.X / b.X, a.Y / b.Y, a.Width / b.Width, a.Height / b.Height, a.Rotation / b.Rotation);

    }
}
