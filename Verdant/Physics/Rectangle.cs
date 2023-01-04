using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using Verdant.UI;

namespace Verdant.Physics
{
    /// <summary>
    /// A rectangle shape, which can rotate.
    /// </summary>
    public class Rectangle : Shape
    {

        Vec2 refDir;
        internal float Length { get; set; }
        internal float Width { get; set; }
        // The angle of the Rectangle's rotation.
        public float Angle { get; set; }
        Matrix rotMat;

        /// <summary>
        /// Initialize a new Rectangle shape.
        /// </summary>
        /// <param name="x1">The X component of the Rectangle's start position (centered).</param>
        /// <param name="y1">The Y component of the Rectangle's start position.</param>
        /// <param name="x2">The X component of the Rectangle's end position (centered).</param>
        /// <param name="y2">The Y component of the Rectangle's end position.</param>
        /// <param name="width">The width of the Rectangle.</param>
        public Rectangle(float x1, float y1, float x2, float y2, float width)
        {
            Vertices = new Vec2[4];
            Vertices[0] = new Vec2(x1, y1);
            Vertices[1] = new Vec2(x2, y2);

            Dir = (Vertices[1] - Vertices[0]).Unit();
            refDir = Dir.Copy();
            Length = (Vertices[1] - Vertices[0]).Magnitude();

            Width = width;

            Vertices[2] = Vertices[1] + (Dir.Normal() * Width);
            Vertices[3] = Vertices[2] + (Dir.Normal() * -Length);

            Position = Vertices[0] + (Dir * Length / 2) + (Dir.Normal() * Width / 2);
            Angle = 0;
            rotMat = new Matrix(2, 2);
        }

        /// <summary>
        /// Update the Rectangle's vertices according to the current position and rotation.
        /// </summary>
        public void CalculateVertices()
        {
            rotMat = PhysicsMath.CalculateRotMatrix(Angle);
            Dir = (rotMat * refDir).Unit();

            Vertices[0] = Position + (Dir * -Length / 2) + (Dir.Normal() * Width / 2);
            Vertices[1] = Position + (Dir * -Length / 2) + (Dir.Normal() * -Width / 2);
            Vertices[2] = Position + (Dir * Length / 2) + (Dir.Normal() * -Width / 2);
            Vertices[3] = Position + (Dir * Length / 2) + (Dir.Normal() * Width / 2);
        }

        public override void Draw(SpriteBatch spriteBatch, Camera camera, Color color)
        {
            Renderer.DrawLine(spriteBatch,
                camera,
                Vertices[0],
                Vertices[1],
                color
                );
            Renderer.DrawLine(spriteBatch,
                camera,
                Vertices[1],
                Vertices[2],
                color
                );
            Renderer.DrawLine(spriteBatch,
                camera,
                Vertices[2],
                Vertices[3],
                color
                );
            Renderer.DrawLine(spriteBatch,
                camera,
                Vertices[3],
                Vertices[0],
                color
                );
        }

    }
}
