using System;

namespace Verdant
{

    public enum TransformStateBlendMode
    {
        Add,
        Multiply,
        Override,
    }

    /// <summary>
    /// Modifies the way an Entity, Particle, etc. is rendered to the screen.
    /// Effect can be either multiplicative (a.k.a "relative", Multiply=true) or absolute (Multiply=false).
    /// </summary>
    public class TransformState
    {

        // Determines the way the TransformState's effects are applied.
        public TransformStateBlendMode BlendMode { get; set; }

        // The position of the TransformState.
        public Vec2 Position { get; set; }
        // The width of the TransformState.
        public float Width { get; set; }
        // The height of the TransformState.
        public float Height { get; set; }
        // The rotation angle of the TransformState.
        public float Angle { get; set; }

        /// <summary>
        /// Initialize a new TransformState.
        /// By default, its properties will be initialized to not have any effect.
        /// </summary>
        /// <param name="blendMode">Determines the TransformState's blend mode.</param>
        public TransformState(TransformStateBlendMode blendMode)
        {
            BlendMode = blendMode;

            // defaults to 1 if multiplicative
            if (BlendMode == TransformStateBlendMode.Multiply)
            {
                Position = new Vec2(1, 1);
                Width = 1;
                Height = 1;
                Angle = 1;
            }
            else
            {
                Position = new Vec2(0, 0);
            }
        }

        /// <summary>
        /// Initialize a new TransformState.
        /// </summary>
        /// <param name="position">The position of the TransformState.</param>
        /// <param name="width">The width of the TransformState.</param>
        /// <param name="height">The height of the TransformState.</param>
        /// <param name="angle">The rotation angle of the TransformState.</param>
        /// <param name="blendMode">Determiens the TransformState's blend mode.</param>
        public TransformState(Vec2 position, float width, float height, float angle, TransformStateBlendMode blendMode)
        {
            Position = position;
            Width = width;
            Height = height;
            Angle = angle;
            BlendMode = blendMode;
        }

        /// <summary>
        /// Create a new TransformState with the same properties as this one.
        /// </summary>
        /// <returns>A new TransformState.</returns>
        public TransformState Copy()
        {
            TransformState newState = new(BlendMode)
            {
                Position = Position.Copy(),
                Width = Width,
                Height = Height,
                Angle = Angle,
            };
            return newState;
        }

        public static TransformState operator +(TransformState a, TransformState b) => new TransformState(a.Position + b.Position, a.Width + b.Width, a.Height + b.Height, a.Angle + b.Angle, a.BlendMode);
        public static TransformState operator -(TransformState a, TransformState b) => new TransformState(a.Position - b.Position, a.Width - b.Width, a.Height - b.Height, a.Angle - b.Angle, a.BlendMode);
        public static TransformState operator *(TransformState a, TransformState b) => new TransformState(a.Position * b.Position, a.Width * b.Width, a.Height * b.Height, a.Angle * b.Angle, a.BlendMode);
        public static TransformState operator /(TransformState a, TransformState b) => new TransformState(a.Position / b.Position, a.Width / b.Width, a.Height / b.Height, a.Angle / b.Angle, a.BlendMode);

    }
}
