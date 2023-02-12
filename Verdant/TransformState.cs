using System;

namespace Verdant
{
    public class TransformState
    {

        public bool Multiply { get; private set; } = true;

        public Vec2 Position { get; set; }
        public float Width { get; set; }
        public float Height { get; set; }
        public float Angle { get; set; }

        public TransformState(bool mutliply = true)
        {
            Multiply = mutliply;

            // defaults to 1 if multiplicative
            if (Multiply)
            {
                Position = new Vec2(1, 1);
                Width = 1;
                Height = 1;
                Angle = 1;
            }
        }

        public TransformState(Vec2 position, float width, float height, float angle, bool multiply = true)
        {
            Position = position;
            Width = width;
            Height = height;
            Angle = angle;
            Multiply = multiply;
        }

        public TransformState Copy()
        {
            TransformState newState = new(Multiply);
            newState.Position = Position.Copy();
            newState.Width = Width;
            newState.Height = Height;
            newState.Angle = Angle;
            return newState;
        }

        public static TransformState operator +(TransformState a, TransformState b) => new TransformState(a.Position + b.Position, a.Width + b.Width, a.Height + b.Height, a.Angle + b.Angle);
        public static TransformState operator -(TransformState a, TransformState b) => new TransformState(a.Position - b.Position, a.Width - b.Width, a.Height - b.Height, a.Angle - b.Angle);
        public static TransformState operator *(TransformState a, TransformState b) => new TransformState(a.Position * b.Position, a.Width * b.Width, a.Height * b.Height, a.Angle * b.Angle);
        public static TransformState operator /(TransformState a, TransformState b) => new TransformState(a.Position / b.Position, a.Width / b.Width, a.Height / b.Height, a.Angle / b.Angle);

    }
}
